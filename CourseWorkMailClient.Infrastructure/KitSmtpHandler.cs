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
using System.IO;
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

        public async Task SendMessage(Letter messageToSend, bool useCryptography)
        {
            var message = new MimeMessage();

            message.Headers.Add(new Header(HeaderId.Summary, "localMessage"));

            message.From.Add(new MailboxAddress(login, login));

            messageToSend.Receivers.ForEach(h => message.To.Add(new MailboxAddress(h.Email, h.Email)));

            message.Subject = messageToSend.Subject;

            var builder = new BodyBuilder();

            builder.TextBody = messageToSend.Content;

            messageToSend.Attachments?.ForEach(h => builder.Attachments.Add(h.Name));

            message.Body = builder.ToMessageBody();

            if (useCryptography)
            {
                var md5 = new CryptoMD5();
                var des = new CryptoDES();

                md5.CreateNewRsaKey();
                des.CreateNewRsaKey();

                for (int i = 0; i < message.BodyParts.Count(); i++)
                {
                    var item = message.BodyParts.ElementAt(i);
                    if (item is TextPart)
                    {
                        var textItem = (TextPart)item;
                        var textInBytes = Encoding.UTF8.GetBytes(textItem.Text);

                        textItem.Text = Convert.ToBase64String(des.EncryptUsingDes(textInBytes));
                        textItem.ContentMd5 = Convert.ToBase64String(md5.GetHash(textInBytes));
                    }
                }

                messageToSend.MD5RsaKey = md5.GetRsaKey();
                messageToSend.DESRsaKey = des.GetRsaKey();

                message.Headers.Add(HeaderId.Encrypted, messageToSend.DESRsaKey.PublicKey);
                message.Headers.Add(HeaderId.ContentMd5, messageToSend.MD5RsaKey.PublicKey);
            }

            var fileMesPath = Path.Combine("Letters", message.MessageId.Substring(0, message.MessageId.IndexOf('@')) + ".mes");

            message.WriteTo(fileMesPath);
            messageToSend.PathToFullMessageFile = fileMesPath;

            HandlerService.repo.AddMessage(messageToSend);
            HandlerService.repo.SaveChanged();

            await client.SendAsync(message);
        }

        public void Logout()
        {
            client.Disconnect(true);
        }
    }
}
