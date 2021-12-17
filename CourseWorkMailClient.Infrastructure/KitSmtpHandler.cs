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

        public KitSmtpHandler(string login, string password)
        {
            client = new SmtpClient();
            client.Connect("smtp." + GetDataService.MailServers.First(h=>login.Contains(h.Key)).Value, 465, SecureSocketOptions.SslOnConnect);
            client.Authenticate(login, password);
        }

        public async Task SendMessage(Letter messageToSend)
        {
            var message = new MimeMessage();

            var user = GetDataService.ActualUser;

            message.From.Add(new MailboxAddress(user.Login, user.Login));

            messageToSend.Receivers.ForEach(h => message.To.Add(new MailboxAddress(h.Email, h.Email)));

            messageToSend.Senders = new List<Interlocutor>();
            messageToSend.Senders.Add(HandlerService.Repository.GetOrCreateInterlocutor(user.Login));

            message.Subject = messageToSend.Subject;

            var builder = new BodyBuilder();


            var receiver = HandlerService.Repository.GetInterlocutor(messageToSend.Receivers.First().Email, true);

            //Создаем ключи для отправителя и записываем их, если ключи отсутствуют
            PrepareData.CreateUsersKeys(receiver);

            if (receiver.LastDESRsaKeyId != null && receiver.LastMD5RsaKeyId != null && messageToSend.Receivers.Count == 1)
            {
                var md5 = new CryptoMD5();
                var des = new CryptoDES();

                md5.CreateNewRsaKey();
                des.CreateNewRsaKey();

                //Ключ для создания подписи отправителя
                md5.SetRsaKey(receiver.UserLastMD5RsaKey.PrivateKey);
                //Ключ для шифрования получателя
                des.SetRsaKey(receiver.LastDESRsaKey.PublicKey);


                var textInBytes = Encoding.UTF8.GetBytes(messageToSend.Content);

                builder.HtmlBody = Convert.ToBase64String(des.EncryptUsingDes(textInBytes).Concat(md5.GetHash(textInBytes)).ToArray());
                //textItem.ContentMd5 = Convert.ToBase64String(md5.GetHash(textInBytes));

                messageToSend.Attachments?.ForEach(h => {
                    using var memoryStream = new MemoryStream(File.ReadAllBytes(h.Name));
                    var contentBytes = memoryStream.ToArray();
                    builder.Attachments.Add(h.Name, des.EncryptUsingDes(contentBytes).Concat(md5.GetHash(contentBytes)).ToArray());
                });


                messageToSend.MD5RsaKey = receiver.UserLastMD5RsaKey;
                messageToSend.DESRsaKey = receiver.LastDESRsaKey;
            }
            else
            {
                builder.HtmlBody = messageToSend.Content;

                messageToSend.Attachments?.ForEach(h => builder.Attachments.Add(h.Name));
            }



            message.Body = builder.ToMessageBody();

            message.Headers.Add(HeaderId.Encrypted, receiver.UserLastDESRsaKey.PublicKey);
            message.Headers.Add(HeaderId.Summary, receiver.UserLastMD5RsaKey.PublicKey);

            var fileMesPath = Path.Combine("Letters", Guid.NewGuid().ToString() + ".mes");

            message.WriteTo(fileMesPath);
            messageToSend.PathToFullMessageFile = fileMesPath;

            HandlerService.KitImapHandler.AppendMessage(message, GetDataService.Folders.First(h=>h.FolderTypeId == 2).Source);

/*            HandlerService.Repository.AddMessage(messageToSend);
            HandlerService.Repository.SaveChanged();*/

            if (!client.IsConnected)
            {
                client.Connect("smtp." + GetDataService.MailServers.First(h => GetDataService.ActualUser.Login.Contains(h.Key)).Value, 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate(GetDataService.ActualUser.Login, GetDataService.ActualUser.Login);
            }
            await client.SendAsync(message);
        }

        public void Logout()
        {
            client.Disconnect(true);
        }
    }
}
