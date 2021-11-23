using AutoMapper;
using CourseWorkMailClient.Domain;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Infrastructure
{
    public class ImapHandler
    {
        public ImapClient client;
        private IMapper mapper;

        public ImapHandler(string login, string password)
        {
            client = new ImapClient("imap.gmail.com", 993, true);
            client.Login(login, password, AuthMethod.Login);

            var config = new MapperConfiguration(ctg => ctg.CreateMap<MailMessage, CustomMessage>()
                .ForMember(dest => dest.Subject, act => act.MapFrom(src => src.Subject))
                .ForMember(dest => dest.From, act => act.MapFrom(src => src.From.Address))
                .ForMember(dest => dest.Content, act => act.MapFrom(src => src.Body))
                .ForMember(dest => dest.Attachments, act => act.MapFrom(src => src.Attachments.Select(h => GetNameAttachemnt(h))))
                .ForMember(dest => dest.To, act => act.MapFrom(src => src.To.Select(h => h.Address)))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date())));
            mapper = new Mapper(config);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private string GetNameAttachemnt(Attachment attachment)
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
        }

        public AlternateView GetAlternateViewContent(MailMessage src, string type = "text/html")
        {
            foreach (var item in src.AlternateViews)
                if (item.ContentType.MediaType == type)
                    return item;

            return null;
        }

        public CustomMessage GetMessage(uint id)
        {
            var src = client.GetMessage(id);
            return GetMessage(id, src);
        }

        public CustomMessage GetMessage(uint id, MailMessage src)
        {
            var mes = mapper.Map<CustomMessage>(src);

            mes.Id = id;

            var altView = GetAlternateViewContent(src);
            if (altView != null)
            {
                using var reader = new StreamReader(altView.ContentStream);
                mes.HtmlContent = reader.ReadToEnd();
            }
            
            return mes;
        }

        public List<CustomMessage> GetMessages()
        {
            var uids = client.Search(SearchCondition.All()).ToList();
            var list = client.GetMessages(uids).ToList();
            var messes = new List<CustomMessage>();

            for (int i = 0; i < uids.Count; i++)
            {
                messes.Add(GetMessage(uids[i], list[i]));
            }

            return messes;
        }

        public void DownloadAttachment(string name, string path, uint mesId)
        {
            var src = client.GetMessage((uint)mesId);

            DownloadAttachment(name, path, src);
        }

        public void DownloadAttachment(string name, string path, MailMessage src)
        {
            var item = src.Attachments.Single(h => h.Name == name);

            var file = new FileInfo(path + item.Name);
            var filePath = file.FullName;
            for (int i = 1; File.Exists(filePath); i++)
                filePath = path + file.Name.Replace(file.Extension, "") + "_" + i + file.Extension;

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            var b = item.ContentStream.ReadByte();
            while (b != -1)
            {
                fileStream.WriteByte((byte)b);
                b = item.ContentStream.ReadByte();
            }
            fileStream.Close();
        }

        public void DownloadAttachments(List<string> names, string path, int mesId)
        {
            var src = client.GetMessage((uint)mesId);

            foreach (var item in names)
                DownloadAttachment(item, path, src);
        }
    }
}
