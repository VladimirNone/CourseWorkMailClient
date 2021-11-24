using AutoMapper;
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
        public ImapClient client;
        private IMapper mapper;

        public KitImapHandler(string login, string password)
        {
            client = new ImapClient();
            client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

            client.Authenticate(login, password);

            var config = new MapperConfiguration(ctg =>
            {
                ctg.CreateMap<MimeMessage, CustomMessage>()
                    .ForMember(dest => dest.Subject, act => act.MapFrom(src => src.Subject))
                    .ForMember(dest => dest.From, act => act.MapFrom(src => string.Join(", ", src.From.Select(h => h.Name).ToList())))
                    .ForMember(dest => dest.Froms, act => act.MapFrom(src => src.From.Select(h => h.Name).ToList()))
                    .ForMember(dest => dest.Content, act => act.MapFrom(src => src.HtmlBody))
                    .ForMember(dest => dest.Attachments, act => act.MapFrom(src => src.Attachments.Select(h => h.ContentDisposition.FileName)))
                    .ForMember(dest => dest.To, act => act.MapFrom(src => src.To.Select(h => h.Name)))
                    .ForMember(dest => dest.Source, act => act.MapFrom(src => src))
                    .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date.DateTime));

                ctg.CreateMap<MailFolder, CustomFolder>()
                    .ForMember(dest => dest.Source, act => act.MapFrom(src => src))
                    .ForMember(dest => dest.Title, act => act.MapFrom(src => GetParsedName(src.Name)));
            });

            mapper = new Mapper(config);
        }

        private string GetParsedName(string folderName)
        {
            return folderName == "INBOX" ? "Входящие" : folderName;
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public CustomFolder GetFolder(MailFolder folder)
        {
            return mapper.Map<CustomFolder>(folder);
        }

        public List<CustomFolder> GetFolders()
        {
            var folders = client.GetFolders(client.PersonalNamespaces[0]).ToList();

            var gmailFolder = folders.FirstOrDefault(h => h.Name == "[Gmail]");
            if(gmailFolder != null)
                folders.Remove(gmailFolder);

            return new List<CustomFolder>(folders.Select(h => mapper.Map<CustomFolder>(h)));
        }

        public CustomMessage GetMessage(uint id, IMailFolder folder)
        {
            var mimeMes = GetMimeMessage(id, folder);

            var mes = mapper.Map<CustomMessage>(mimeMes);
            mes.Id = id;
            mes.Date = mimeMes.Date.DateTime;

            return mes;
        }

        public List<CustomMessage> GetMessages(IMailFolder folder)
        {
            var uids = folder.Search(SearchQuery.All);

            var customMessages = new List<CustomMessage>();

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
    }
}

