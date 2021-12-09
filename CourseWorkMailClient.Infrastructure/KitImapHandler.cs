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
using System.Text;
using System.Threading.Tasks;


/*            var des = new CryptoDES();
            var encryptedContent = des.EncryptUsingDes(Encoding.UTF8.GetBytes(Message.Content));
            var base64EncrCont = Convert.ToBase64String(encryptedContent);

            var decryptedContent = Encoding.UTF8.GetString(des.DecryptUsingDes(encryptedContent));
            var dectyptContentFromBase64 = Encoding.UTF8.GetString(des.DecryptUsingDes(Convert.FromBase64String(base64EncrCont)));*/

namespace CourseWorkMailClient.Infrastructure
{
    public class KitImapHandler
    {
        private ImapClient client;
        public bool IsConnected { get => client.IsConnected; }

        public KitImapHandler(string login, string password)
        {
            client = new ImapClient();
            client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

            client.Authenticate(login, password);
        }

        public void LoadLastLetters()
        {
            ParallelFunction.LoadLastLetter(HandlerService.mapper.Map<Folder>(client.GetFolder(SpecialFolder.All)), 50);
        }

        public void OpenFolder(Folder folder)
        {
            if (folder.Source != null)
            {
                folder.Source.Open(FolderAccess.ReadWrite);
                folder.CountOfMessage = folder.Source.Count;

                GetDataService.uniqueIdsLastFolder = folder.Source.Search(SearchQuery.All).ToList();
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

            var gmailFolder = folders.FirstOrDefault(h => h.Name == "[Gmail]");
            if (gmailFolder != null)
                folders.Remove(gmailFolder);

            var customFolders = new List<Folder>(folders.Select(h => GetCustedFolder(h)));

            return customFolders;
        }

        public void MoveMessage(MimeMessage message, IMailFolder newMessageFolder)
        {
/*            if (!newMessageFolder.IsOpen)
                newMessageFolder.Open(FolderAccess.ReadWrite);*/
            newMessageFolder.Append(message);
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
            var messages = folder.Source.Fetch(GetDataService.uniqueIdsCurrentPage, MessageSummaryItems.Headers);

            var letters = new List<Letter>();

            foreach (var item in messages)
            {
                var message = HandlerService.mapper.Map<MimeMessage, Letter>(new MimeMessage(item.Headers));
                message.UniqueId = (int)item.UniqueId.Id;
                letters.Add(message);
            }

            return letters;
        }

        public MimeMessage GetMimeMessage(uint id, IMailFolder folder)
        {
            var mes = folder.GetMessage(new UniqueId(id));

            if (mes.Headers.Contains(HeaderId.Summary))
            {
                for (int i = 0; i < mes.BodyParts.Count(); i++)
                {
                    var item = mes.BodyParts.ElementAt(i);
                    var textItem = (TextPart)item;
                    //textItem.Text = Encoding.UTF8.GetString(CryptoDES.DecryptUsingDes(Convert.FromBase64String(textItem.Text), "One" + i));
                }
            }

            return mes;
        }

        public MimeMessage GetMimeMessage(string path)
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

        public void DownloadAttachments(List<string> names, string path, MimeMessage src)
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

