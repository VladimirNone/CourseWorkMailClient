using AutoMapper;
using CourseWorkMailClient.Domain;
using Lab6;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using MimeKit.Cryptography;
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

        public async Task SendMessage(Letter messageToSend)
        {
            var message = new MimeMessage();

            message.Headers.Add(new Header(HeaderId.Summary, "localMessage"));

            message.From.Add(new MailboxAddress("", login));

            //messageToSend.To.ForEach(h => message.To.Add(new MailboxAddress("", h)));

            message.Subject = messageToSend.Subject;

            var builder = new BodyBuilder();

            builder.TextBody = messageToSend.Content;

            //messageToSend.Attachments?.ForEach(h => builder.Attachments.Add(h));

            message.Body = builder.ToMessageBody();

            for (int i = 0; i < message.BodyParts.Count(); i++)
            {
                var item = message.BodyParts.ElementAt(i);
                if (item is TextPart)
                {
                    var textItem = (TextPart)item;
                    textItem.Text = Convert.ToBase64String(CryptoDES.EncryptUsingDes(Encoding.UTF8.GetBytes(textItem.Text), "One" + i));
                }
            }

            await client.SendAsync(message);
        }

        public void Logout()
        {
            client.Disconnect(true);
        }
    }
}
