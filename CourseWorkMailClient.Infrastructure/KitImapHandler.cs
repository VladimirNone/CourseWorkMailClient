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

            var config = new MapperConfiguration(ctg => ctg.CreateMap<MimeMessage, CustomMessage>()
                .ForMember(dest => dest.Subject, act => act.MapFrom(src => src.Subject))
                .ForMember(dest => dest.From, act => act.MapFrom(src => string.Join(", ", src.From.Select(h => h.Name).ToList())))
                .ForMember(dest => dest.Froms, act => act.MapFrom(src => src.From.Select(h => h.Name).ToList()))
                .ForMember(dest => dest.Content, act => act.MapFrom(src => src.HtmlBody))
                .ForMember(dest => dest.Attachments, act => act.MapFrom(src => src.Attachments.Select(h => h.ContentDisposition.FileName)))
                .ForMember(dest => dest.To, act => act.MapFrom(src => src.To.Select(h => h.Name)))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date.DateTime)));
            mapper = new Mapper(config);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /*        private string GetNameAttachemnt(Attachment attachment)
                {
                    var name = "";

                    if (attachment.Name.Contains("=?UTF-8?B?"))
                    {
                        var parts = attachment.Name.Split(new[] { "=?UTF-8?B?" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < parts.Length; i++)
                        {
                            var length = parts[i].IndexOf('?') == -1 ? parts[i].Length : parts[i].IndexOf('?');
                            parts[i] = parts[i].Substring(0, length);
                            name += Base64Decode(parts[i]);
                        }
                    }
                    else
                        name = attachment.Name;

                    return name;
                }*/

        public CustomMessage GetMessage(uint id, bool readOnly = true, SpecialFolder folder = SpecialFolder.All)
        {
            var mimeMes = GetMimeMessage(id, readOnly, folder);

            var mes = mapper.Map<CustomMessage>(mimeMes);
            mes.Id = id;
            mes.Date = mimeMes.Date.DateTime;

            return mes;
        }

        public List<CustomMessage> GetMessages(SpecialFolder folder = SpecialFolder.All)
        {
            var allFolder = client.GetFolder(folder);
            allFolder.Open(FolderAccess.ReadOnly);

            var uids = allFolder.Search(SearchQuery.All);

            var customMessages = new List<CustomMessage>();

            foreach (var uid in uids)
                customMessages.Add(GetMessage(uid.Id, true, folder));

            return customMessages;
        }

        public MimeMessage GetMimeMessage(uint id, bool readOnly = true, SpecialFolder folder = SpecialFolder.All)
        {
            var dir = client.GetFolder(folder);
            if (!dir.IsOpen)
                dir.Open(readOnly ? FolderAccess.ReadOnly : FolderAccess.ReadWrite);

            return dir.GetMessage(new UniqueId(id));
        }

        public void DownloadAttachment(string name, string path, uint mesId)
        {
            var src = GetMimeMessage(mesId);

            DownloadAttachment(name, path, src);
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

        public void DownloadAttachments(List<string> names, string path, uint mesId)
        {
            var src = GetMimeMessage(mesId);

            foreach (var item in names)
                DownloadAttachment(item, path, src);
        }
    }
}

