using CourseWorkMailClient.Domain;
using Lab6;
using MailKit;
using MimeKit;
using MimeKit.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CourseWorkMailClient.Infrastructure
{
    public static class GetDataService
    {
        public static Dictionary<string, string> MailServers { get; set; } = new Dictionary<string, string>()
        {
            { "yandex.ru", "yandex.ru" },
            { "gmail.com", "gmail.com" },
            { "mail.ru", "mail.ru" },
        };

        public static string PathToJsonFile { get => "users.json"; }
        public static Dictionary<string, string> UserDb { get; set; }

        public static CustomNotifyCollectionCollection<Letter> Letters { get; set; } = new CustomNotifyCollectionCollection<Letter>();

        public static CustomNotifyCollectionCollection<Folder> Folders;
        private static Folder actualFolder;
        public static Folder ActualFolder
        {
            get => actualFolder;
            set
            {
                if(actualFolder != null && actualFolder != value && actualFolder.Source != null)
                    HandlerService.KitImapHandler.CloseFolder(actualFolder);

                actualFolder = value;
                Pagination.Page = 1;
                Pagination.ChangePage(0);
                if(ChangeActualFolder != null)
                    ChangeActualFolder();
            }
        }

        public static Action ChangeActualFolder;

        public static MailServer ActualMailServer { get; set; }
        public static User ActualUser { get; set; }

        public static PaginationService Pagination { get; set; } = new PaginationService();

        public static List<UniqueId> uniqueIdsLastFolder { get; set; }
        public static List<UniqueId> uniqueIdsCurrentPage { get; set; }

        static GetDataService()
        {
            Pagination.ChangingPage += (curPage, itemsOnPage) =>
            {
                uniqueIdsCurrentPage = uniqueIdsLastFolder.Skip((curPage - 1) * itemsOnPage).Take(itemsOnPage).ToList();

                Letters.Reset(GetMessages(ActualFolder));
            };
        }

        public static void AddRowToUserDbFile(string key, string value)
        {
            UserDb.Add(key, value);
            var data = JsonConvert.SerializeObject(UserDb, Formatting.Indented);
            using var writer = File.CreateText(PathToJsonFile);
            writer.Write(data);
        }

        public static bool OpenFolder(Folder folder)
        {
            if(folder.FolderTypeId == HandlerService.Repository.GetFolderTypeId("Отправленные"))
            {
                MessageBox.Show("В данный момент, чтение отправленных не поддерживается");
                return false;
            }

            if(folder.Source != null)
            {
                HandlerService.KitImapHandler.OpenFolder(folder);

                //удаляем письма, к-е были удалены или перемещены из этой папки
                var uids = HandlerService.Repository.GetUniqueIds(folder);
                var uniqueIdsLastFolderFromDb = uids.Select(h => new UniqueId((uint)h)).Reverse().ToList();
                var removedUids = uniqueIdsLastFolderFromDb.Except(uniqueIdsLastFolder).Select(h=>(int)h.Id).ToList();
                HandlerService.Repository.RemoveMessages(removedUids, folder);
                HandlerService.Repository.SaveChanged();
            }
            else
            {
                folder.CountOfMessage = HandlerService.Repository.GetCountOfMessages(folder.Id);
                var uids = HandlerService.Repository.GetUniqueIds(folder);
                uniqueIdsLastFolder = uids.Select(h => new UniqueId((uint)h)).Reverse().ToList();
                Pagination.MaxCountOfPage = (int)folder.CountOfMessage / Pagination.ItemsOnPage + ((int)folder.CountOfMessage % Pagination.ItemsOnPage == 0 ? 0 : 1);
            }

            return true;
        }

        public static List<string> GetMovableFolders()
        {
            var unMovableFolders = new List<string>() { "" };
            var folderNames = Folders.Select(h => h.Title).ToList();

            folderNames.RemoveAll(h => unMovableFolders.Contains(h));

            return folderNames;
        }

        public static List<Folder> GetFolders()
        {
            var folders = new List<Folder>();
            if (HandlerService.KitImapHandler == null || !HandlerService.KitImapHandler.IsConnected)
            {
                folders = HandlerService.Repository.GetFolders(ActualMailServer);
            }
            else
            {
               folders = HandlerService.KitImapHandler.GetServerFolders();

                HandlerService.Repository.SelectAndAddNewFolders(folders, ActualMailServer);
                HandlerService.Repository.SaveChanged();

                var dbFolders = HandlerService.Repository.GetFolders(ActualMailServer);

                foreach (var item in dbFolders)
                {
                    item.Source = folders.FirstOrDefault(h => h.Title == item.Title)?.Source;
                }

                folders = dbFolders;
            }
            
            return folders;
        }

        /// <summary>
        /// Возвращает полную версию сообщения
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static Letter GetMessage(Letter Message, Folder folder)
        {
            var mesFromDb = HandlerService.Repository.GetMessage(Message.UniqueId, folder, lightVersion: false);

            //в теории никогда не будет использоваться, но используется((
            if (mesFromDb == null)
            {
                mesFromDb = HandlerService.KitImapHandler.GetFullMessage(Message, folder);
                HandlerService.Repository.AddMessage(mesFromDb);
                HandlerService.Repository.SaveChanged();
            }

            if (mesFromDb.PathToFullMessageFile != null)
            {
                mesFromDb.Source = HandlerService.KitImapHandler.GetMimeMessage(mesFromDb.PathToFullMessageFile);
            }
            else
            {
                mesFromDb = HandlerService.KitImapHandler.GetFullMessage(mesFromDb, folder);
                HandlerService.Repository.SaveChanged();
            }

            //Расшифровка
            if(mesFromDb.MD5RsaKeyId != null && mesFromDb.DESRsaKeyId != null)
            {
                for (int i = 0; i < mesFromDb.Source.BodyParts.Count(); i++)
                {
                    var item = mesFromDb.Source.BodyParts.ElementAt(i);
                    if (item is TextPart)
                    {
                        var textItem = (TextPart)item;

                        var des = new CryptoDES();
                        des.CreateNewRsaKey();
                        des.SetRsaKey(mesFromDb.DESRsaKey.PrivateKey);

                        textItem.Text = Encoding.UTF8.GetString(des.DecryptUsingDes(Convert.FromBase64String(textItem.Text)));

                        var md5 = new CryptoMD5();
                        md5.CreateNewRsaKey();
                        md5.SetRsaKey(mesFromDb.MD5RsaKey.PublicKey);

/*                        var valid = md5.CheckHash(Convert.FromBase64String(textItem.ContentMd5), Encoding.UTF8.GetBytes(textItem.Text));
                        if (!valid)
                        {
                            MessageBox.Show("Проверка подписью прошла неудачно");
                        }*/
                    }
                    else if(item is MimePart)
                    {
                        var mimePart = (MimePart)item;

                        var des = new CryptoDES();
                        des.CreateNewRsaKey();
                        des.SetRsaKey(mesFromDb.DESRsaKey.PrivateKey);

                        using var ms = new MemoryStream();
                        mimePart.Content.DecodeTo(ms);

                        var decryptedBytes = des.DecryptUsingDes(ms.ToArray());

                        //using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                        var resMS = new MemoryStream(decryptedBytes);
                        mimePart.Content = new MimeContent(resMS);

                        

                        var md5 = new CryptoMD5();
                        md5.CreateNewRsaKey();
                        md5.SetRsaKey(mesFromDb.MD5RsaKey.PublicKey);

/*                        var valid = md5.CheckHash(Convert.FromBase64String(mimePart.ContentMd5), decryptedBytes);
                        if (!valid)
                        {
                            MessageBox.Show("Проверка подписью прошла неудачно");
                        }*/
                    }
                }
            }

            HandlerService.mapper.Map(mesFromDb.Source, mesFromDb);

            //Отмечаем, что письмо было прочитано
            HandlerService.KitImapHandler.MessageWasSeen(Message.UniqueId, folder);
            Message.Seen = true;
            HandlerService.Repository.SaveChanged();
            

            return mesFromDb;
        }

        public static List<Letter> GetMessages(Folder folder)
        {
            List<Letter> messages = null;
            if (folder.Source == null)
            {
                messages = HandlerService.Repository.GetMessages(folder.Id, uniqueIdsCurrentPage.Select(h => (int)h.Id).ToList());
            }
            else
            {
                messages = HandlerService.KitImapHandler.GetMessages(folder);
                messages = HandlerService.Repository.SelectAndAddNewLetters(messages, folder);
            }

            return messages.OrderByDescending(h=>h.UniqueId).ToList();
        }
    }
}
