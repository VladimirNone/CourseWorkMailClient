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
            client = new SmtpClient();
            client.Connect("smtp." + GetDataService.MailServers.First(h=>login.Contains(h.Key)).Value, 465, SecureSocketOptions.SslOnConnect);
            client.Authenticate(login, password);
            this.login = login;
        }

        public async Task SendMessage(Letter messageToSend)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(login, login));

            messageToSend.Receivers.ForEach(h => message.To.Add(new MailboxAddress(h.Email, h.Email)));

            messageToSend.Senders = new List<Interlocutor>();
            messageToSend.Senders.Add(HandlerService.Repository.GetOrCreateInterlocutor(login));

            message.Subject = messageToSend.Subject;

            var builder = new BodyBuilder();

            builder.TextBody = messageToSend.Content;

            messageToSend.Attachments?.ForEach(h => builder.Attachments.Add(h.Name));

            message.Body = builder.ToMessageBody();


            var receiver = HandlerService.Repository.GetInterlocutor(messageToSend.Receivers.First().Email, true);
            var sender = HandlerService.Repository.GetInterlocutor(messageToSend.Senders.First().Email, true);

            //Создаем ключи для отправителя и записываем их, если ключи отсутствуют
            if (sender.LastDESRsaKeyId == null || sender.LastMD5RsaKeyId == null)
            {
                var md5Sender = new CryptoMD5();
                var desSender = new CryptoDES();

                md5Sender.CreateNewRsaKey();
                desSender.CreateNewRsaKey();

                sender.LastMD5RsaKey = md5Sender.GetRsaKey();
                sender.LastDESRsaKey = desSender.GetRsaKey();
            }

            if (receiver.LastDESRsaKeyId != null && receiver.LastMD5RsaKeyId != null && messageToSend.Receivers.Count == 1)
            {
                var md5 = new CryptoMD5();
                var des = new CryptoDES();

                md5.CreateNewRsaKey();
                des.CreateNewRsaKey();

                //Ключ для создания подписи отправителя
                md5.SetRsaKey(sender.LastMD5RsaKey.PrivateKey);
                //Ключ для шифрования получателя
                des.SetRsaKey(receiver.LastDESRsaKey.PublicKey);

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

                messageToSend.MD5RsaKey = sender.LastMD5RsaKey;
                messageToSend.DESRsaKey = receiver.LastDESRsaKey;
            }

            message.Headers.Add(HeaderId.Encrypted, sender.LastDESRsaKey.PublicKey);
            message.Headers.Add(HeaderId.Summary, sender.LastMD5RsaKey.PublicKey);

            var fileMesPath = Path.Combine("Letters", Guid.NewGuid().ToString() + ".mes");

            message.WriteTo(fileMesPath);
            messageToSend.PathToFullMessageFile = fileMesPath;

            HandlerService.Repository.AddMessage(messageToSend);
            HandlerService.Repository.SaveChanged();

            var answer = await client.SendAsync(message);
        }

        public void Logout()
        {
            client.Disconnect(true);
        }
    }
}
