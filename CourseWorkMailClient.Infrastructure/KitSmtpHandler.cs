using AutoMapper;
using CourseWorkMailClient.Domain;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Infrastructure
{
    public class KitSmtpHandler
    {
        private SmtpClient client;
        private string login;

        public KitSmtpHandler(string login, string password)
        {
            this.login = login;
            client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            client.Authenticate(login, password);
        }

        public async Task SendMessage(LightMessage messageToSend)
        {
            var message = new MimeMessage();

            message.Headers.Add(new Header(HeaderId.Summary, "localMessage"));

            message.From.Add(new MailboxAddress("", login));

            messageToSend.To.ForEach(h => message.To.Add(new MailboxAddress("", h)));

            message.Subject = messageToSend.Subject;

            var builder = new BodyBuilder();

            builder.TextBody = messageToSend.Content;

            messageToSend.Attachments?.ForEach(h => builder.Attachments.Add(h));

            message.Body = builder.ToMessageBody();

            await client.SendAsync(message);
        }

        public void Logout()
        {
            client.Disconnect(true);
        }
    }
}
