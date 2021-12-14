using AutoMapper;
using CourseWorkMailClient.Data;
using CourseWorkMailClient.Domain;
using MailKit;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CourseWorkMailClient.Infrastructure
{
    public static class HandlerService
    {
        public static IMapper mapper { get; set; }

        public static DbRepository Repository { get; set; }
        public static KitImapHandler KitImapHandler { get; set; }
        public static KitSmtpHandler KitSmtpHandler { get; set; }

        static HandlerService()
        {
            var config = new MapperConfiguration(ctg =>
            {
                ctg.CreateMap<Paragraph, LightParagraph>()
                    .ForMember(dest => dest.Inlines, act => act.MapFrom(src => src.Inlines.Where(h => h is Run).Select(h => mapper.Map<LightRun>(h))));

                ctg.CreateMap<Run, LightRun>();

                ctg.CreateMap<LightRun, Run>();

                ctg.CreateMap<MimeEntity, Attachment>()
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.ContentDisposition.FileName));

                ctg.CreateMap<InternetAddress, Interlocutor>()
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => ((MailboxAddress)src).Address));

                ctg.CreateMap<MailFolder, Folder>()
                    .ForMember(dest => dest.Source, act => act.MapFrom(src => src))
                    .ForMember(dest => dest.FolderTypeId, act => act.MapFrom(src => PrepareData.GetFolderTypeId(src.Attributes)))
                    .ForMember(dest => dest.MailServerId, act => act.MapFrom(src => GetDataService.ActualMailServer.Id))
                    .ForMember(dest => dest.Title, act => act.MapFrom(src => PrepareData.GetParsedFolderName(src.Name)));

                ctg.CreateMap<MimeMessage, Letter>()
                    .ForMember(dest => dest.Subject, act => act.MapFrom(src => src.Subject))
                    .ForMember(dest => dest.From, act => act.MapFrom(src => string.Join(", ", src.From.Mailboxes.Select(h => string.IsNullOrEmpty(h.Name) ? h.Address : h.Name))))
                    .ForMember(dest => dest.To, act => act.MapFrom(src => string.Join(", ", src.To.Mailboxes.Select(h => string.IsNullOrEmpty(h.Name) ? h.Address : h.Name))))
                    .ForMember(dest => dest.Senders, act => act.MapFrom(src => src.From.Select(h => Repository.GetOrCreateInterlocutor(((MailboxAddress)h).Address)).ToList()))
                    .ForMember(dest => dest.Receivers, act => act.MapFrom(src => src.To.Select(h => Repository.GetOrCreateInterlocutor(((MailboxAddress)h).Address)).ToList()))
                    .ForMember(dest => dest.Content, act => act.MapFrom(src => src.HtmlBody ?? src.TextBody))
                    .ForMember(dest => dest.Attachments, act => act.MapFrom(src => src.Attachments.Select(h => mapper.Map<Attachment>(h)).ToList()))
                    .ForMember(dest => dest.Source, act => act.MapFrom(src => src))
                    .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date.DateTime));

                ctg.CreateMap<Folder, Folder>()
                    .ForMember(dest => dest.Source, act => act.Ignore());

                ctg.CreateMap<Letter, Letter>()
                    .ForMember(dest => dest.Id, act => act.Ignore());
            });

            mapper = new Mapper(config);
        }

        public static void Auth(string login)
        {
            //vladimir56545@gmail.com
            //RDk-YDZ-NJb-ucF

            //CourseWork41@gmail.com
            //C9v-EzB-3sT-kfT

            //korolevi4k@yandex.ru
            //XcB-b53-irU-AxV

            //thirdmailforcoursework@yahoo.com
            //vZF-Tkw-r5y-G5p

            Repository = new DbRepository(GetDataService.UserDb[login]);

            GetDataService.ActualUser = Repository.GetUser(login);

            GetDataService.ActualMailServer = GetDataService.ActualUser.MailServer;

            KitImapHandler = new KitImapHandler(GetDataService.ActualUser.Login, GetDataService.ActualUser.Password);
            KitSmtpHandler = new KitSmtpHandler(GetDataService.ActualUser.Login, GetDataService.ActualUser.Password);
        }

        public static void Auth(string login, string password)
        {
            //если вход успешен, то создаем
            KitImapHandler = new KitImapHandler(login, password);
            KitSmtpHandler = new KitSmtpHandler(login, password);

            var connectionStringTemplete = "Server=localhost;Database={0};Trusted_Connection=True;";
            var connectionString = string.Format(connectionStringTemplete, login);

            if (GetDataService.UserDb.Keys.Contains(login))
            {
                Auth(login);
                return;
            }

            GetDataService.AddRowToUserDbFile(login, connectionString);

            Repository = new DbRepository(connectionString);

            GetDataService.ActualUser = Repository.GetUser(login);

            if (GetDataService.ActualUser == null)
            {
                GetDataService.ActualUser = new User() { Login = login, Password = password };

                Repository.AddUser(GetDataService.ActualUser, login[(login.IndexOf('@') + 1)..]);
                Repository.SaveChanged();
            }

            GetDataService.ActualMailServer = GetDataService.ActualUser.MailServer;
        }

        public static void UnAuth()
        {
            if (KitImapHandler != null)
                KitImapHandler.Logout();
            if(KitSmtpHandler != null)
                KitSmtpHandler.Logout();
            Repository.Dispose();
        }
    }
}
