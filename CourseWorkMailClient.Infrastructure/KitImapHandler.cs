using CourseWorkMailClient.Domain;
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

namespace CourseWorkMailClient.Infrastructure
{
    public class KitImapHandler
    {
        private ImapClient client;

        public KitImapHandler(string login, string password)
        {
            client = new ImapClient();
            client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

            client.Authenticate(login, password);

        }

        public IMailFolder CreateNewFolder(string folderName, IMailFolder parentFolder)
        {
            var topFolder = parentFolder ?? client.GetFolder(client.PersonalNamespaces[0]);
            return topFolder.Create(folderName, true);
        }

        public LightFolder GetFolder(IMailFolder folder)
        {
            return HandlerService.mapper.Map<LightFolder>(folder);
        }

        public List<LightFolder> GetFolders()
        {
            var folders = client.GetFolders(client.PersonalNamespaces[0]).ToList();

            var gmailFolder = folders.FirstOrDefault(h => h.Name == "[Gmail]");
            if(gmailFolder != null)
                folders.Remove(gmailFolder);

            return new List<LightFolder>(folders.Select(h => GetFolder(h)));
        }

        public LightMessage GetMessage(uint id, IMailFolder folder)
        {
            var mimeMes = GetMimeMessage(id, folder);

            var mes = HandlerService.mapper.Map<LightMessage>(mimeMes);
            mes.Id = id;
            mes.Date = mimeMes.Date.DateTime;

            return mes;
        }

        public List<LightMessage> GetMessages(IMailFolder folder)
        {
            var uids = folder.Search(SearchQuery.All);
            
            var customMessages = new List<LightMessage>();

            foreach (var uid in uids)
                customMessages.Add(GetMessage(uid.Id, folder));

            return customMessages;
        }

        private MimeMessage GetMimeMessage(uint id, IMailFolder folder)
        {
            return folder.GetMessage(new UniqueId(id));
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

