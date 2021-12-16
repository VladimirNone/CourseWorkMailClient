using CourseWorkMailClient.Domain;
using Lab6;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace CourseWorkMailClient.Infrastructure
{
    public class KitImapHandler
    {
        private ImapClient client;
        public bool IsConnected { get => client.IsConnected; }

        public KitImapHandler(string login, string password)
        {
            client = new ImapClient();
            client.Connect("imap." + GetDataService.MailServers.First(h=>login.Contains(h.Key)).Value, 993, SecureSocketOptions.SslOnConnect);
            client.Authenticate(login, password);

        }

        public void LoadLastLetters(Folder folder)
        {
            if (client.IsConnected)
            {
                var letters = HandlerService.Repository.GetMessages(folder.Id, GetDataService.uniqueIdsCurrentPage.Select(h => (int)h.Id).ToList());

                for (int i = 0; i < letters.Count; i++)
                {
                    if (letters[i].PathToFullMessageFile == null)
                    {
                        letters[i] = GetFullMessage(letters[i], folder);
                    }
                }

                HandlerService.Repository.SaveChanged();
            }
        }

        public void OpenFolder(Folder folder)
        {
            if (folder.Source != null)
            {
                if(!folder.Source.IsOpen)
                    folder.Source.Open(FolderAccess.ReadWrite);
                folder.CountOfMessage = folder.Source.Count;

                GetDataService.uniqueIdsLastFolder = folder.Source.Search(SearchQuery.All).Reverse().ToList();
                GetDataService.Pagination.MaxCountOfPage = (int)folder.CountOfMessage / GetDataService.Pagination.ItemsOnPage + ((int)folder.CountOfMessage % GetDataService.Pagination.ItemsOnPage == 0 ? 0 : 1);
            }
        }

        public void CloseFolder(Folder folder)
        {
            if (folder.Source != null && folder.Source.IsOpen)
            {
                folder.Source.Close();
            }
        }

        public Folder CreateNewFolder(string folderName, IMailFolder parentFolder)
        {
            var topFolder = parentFolder ?? client.GetFolder(client.PersonalNamespaces[0]);
            var newFolder = topFolder.Create(folderName, true);

            return GetCustedFolder(newFolder);
        }

        public Folder RenameFolder(string newFolderName, IMailFolder folder)
        {
            folder.Rename(folder.ParentFolder, newFolderName);
            
            return GetCustedFolder(folder);
        }

        public void DeleteFolder(IMailFolder folder)
        {
            folder.Delete();
        }

        public Folder GetCustedFolder(IMailFolder folder)
        {
            return HandlerService.mapper.Map<Folder>(folder);
        }

        public List<Folder> GetServerFolders()
        {
            var folders = client.GetFolders(client.PersonalNamespaces[0]).ToList();

            if (GetDataService.ActualMailServer.ServerName == GetDataService.MailServers["gmail.com"])
            {
                var gmailFolder = folders.FirstOrDefault(h => h.Name == "[Gmail]");
                if (gmailFolder != null)
                    folders.Remove(gmailFolder);
            }
            else if (GetDataService.ActualMailServer.ServerName == GetDataService.MailServers["yandex.ru"])
            {
                var yandexFolder = folders.FirstOrDefault(h => h.Name == "Outbox");
                if (yandexFolder != null)
                    folders.Remove(yandexFolder);
            }


            var customFolders = new List<Folder>(folders.Select(h => GetCustedFolder(h)));
            
            return customFolders;
        }

        public void AppendMessage(MimeMessage message, IMailFolder newMessageFolder)
        {
            newMessageFolder.Append(message);
        }

        public void MoveMessage(int uid, IMailFolder oldMessageFolder, IMailFolder newMessageFolder)
        {
            oldMessageFolder.MoveTo(new UniqueId((uint)uid), newMessageFolder);
        }

        public void DeleteMessages(List<int> uids, IMailFolder folder)
        {
            folder.AddFlags(uids, MessageFlags.Deleted, false);
            folder.Expunge();
        }

        public void MessageWasSeen(int uid, Folder folder)
        {
            if (folder.Source != null)
            {
                folder.Source.AddFlags(new UniqueId((uint)uid), MessageFlags.Seen, false);
            }
        }

        /// <summary>
        /// Возвращает полную версию сообщения
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public Letter GetFullMessage(Letter letter, Folder folder)
        {
            if (!folder.Source.IsOpen)
                folder.Source.Open(FolderAccess.ReadWrite);

            letter.Source = GetMimeMessage((uint)letter.UniqueId, folder.Source);

            var fileMesPath = Path.Combine("Letters", Guid.NewGuid().ToString() + ".mes");

            letter.Source.WriteTo(fileMesPath);
            letter.PathToFullMessageFile = fileMesPath;

            return letter;
        }

        public List<Letter> GetMessages(Folder folder)
        {
            var messages = folder.Source.Fetch(GetDataService.uniqueIdsCurrentPage, MessageSummaryItems.Headers | MessageSummaryItems.Flags);

            var letters = new List<Letter>();

            foreach (var item in messages)
            {
                var message = HandlerService.mapper.Map<MimeMessage, Letter>(new MimeMessage(item.Headers));
                message.UniqueId = (int)item.UniqueId.Id;
                if (item.Flags.HasValue)
                    message.Seen = item.Flags.Value.HasFlag(MessageFlags.Seen);

                message = PrepareData.ExtractKeysFromServerMes(message);

                letters.Add(message);
            }

            return letters;
        }

        public MimeMessage GetMimeMessage(uint id, IMailFolder folder)
        {
            return folder.GetMessage(new UniqueId(id));
        }

        public static MimeMessage GetMimeMessage(string path)
        {
            return MimeMessage.Load(File.Open(path, FileMode.Open));
        }

        public void DownloadAttachment(string name, string path, MimeMessage src)
        {
            var item = src.Attachments.Single(h => h.ContentDisposition.FileName == name);

            var file = new FileInfo(Path.Combine(path, item.ContentDisposition.FileName));
            var filePath = file.FullName;
            for (int i = 1; File.Exists(filePath); i++)
                filePath = path + file.Name.Replace(file.Extension, "") + "_" + i + file.Extension;

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            if (item is MimePart)
            {
                var part = (MimePart)item;
                
                part.Content.DecodeTo(fileStream);
            }
        }

        public void DownloadAttachments(IEnumerable<string> names, string path, MimeMessage src)
        {
            foreach (var item in names)
                DownloadAttachment(item, path, src);
        }

        public void Logout()
        {
            client.Disconnect(true);
        }
    }
}

