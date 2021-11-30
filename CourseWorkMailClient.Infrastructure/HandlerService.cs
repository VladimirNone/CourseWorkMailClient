using AutoMapper;
using CourseWorkMailClient.Domain;
using MailKit;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CourseWorkMailClient.Infrastructure
{
    public static class HandlerService
    {
        private static List<Letter> actualMessages;
        public static List<Letter> ActualMessages
        {
            get => actualMessages;
            set
            {
                actualMessages = value;
                if(ActualMessagesChanged != null)
                    ActualMessagesChanged(actualMessages);
            }
        }

        public static Action<List<Letter>> ActualMessagesChanged { get; set; }

        public static IMapper mapper { get; set; }

        public static Folder ActualFolder { get; set; }

        static HandlerService()
        {
            var config = new MapperConfiguration(ctg =>
            {
                ctg.CreateMap<Paragraph, LightParagraph>()
                    .ForMember(dest => dest.Inlines, act => act.MapFrom(src => src.Inlines.Where(h => h is Run).Select(h => mapper.Map<LightRun>(h))));

                ctg.CreateMap<Run, LightRun>();

                ctg.CreateMap<LightRun, Run>();

                ctg.CreateMap<MimeMessage, Letter>()
                    .ForMember(dest => dest.Subject, act => act.MapFrom(src => src.Subject))
                    .ForMember(dest => dest.From, act => act.MapFrom(src => string.Join(", ", src.From.Mailboxes.Select(h => string.IsNullOrEmpty(h.Name) ? h.Address : h.Name))))
                    .ForMember(dest => dest.Senders, act => act.MapFrom(src => src.From.Select(h => h.Name).ToList()))
                    .ForMember(dest => dest.Content, act => act.MapFrom(src => src.HtmlBody ?? src.TextBody))
                    .ForMember(dest => dest.LocalMessage, act => act.MapFrom(src => src.Headers.Contains(HeaderId.Summary)))
                    .ForMember(dest => dest.Attachments, act => act.MapFrom(src => src.Attachments.Select(h => h.ContentDisposition.FileName)))
                    .ForMember(dest => dest.To, act => act.MapFrom(src => src.To.Select(h => h.Name)))
                    .ForMember(dest => dest.Source, act => act.MapFrom(src => src))
                    .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date.DateTime));

                ctg.CreateMap<MailFolder, Folder>()
                    .ForMember(dest => dest.Source, act => act.MapFrom(src => src))
                    .ForMember(dest => dest.Title, act => act.MapFrom(src => PrepareData.GetParsedName(src.Name)));
            });

            mapper = new Mapper(config);
        }
              
        public static void Auth(string login, string password)
        {
            //vladimir56545@gmail.com
            //RDk-YDZ-NJb-ucF

            //CourseWork41@gmail.com
            //C9v-EzB-3sT-kfT

            KitImapHandler = new KitImapHandler(login, password);
            KitSmtpHandler = new KitSmtpHandler(login, password);
        }

        public static void UnAuth()
        {
            KitImapHandler.Logout();
            KitSmtpHandler.Logout();
        }

        public static KitImapHandler KitImapHandler { get; set; }
        public static KitSmtpHandler KitSmtpHandler { get; set; }
    }
}
