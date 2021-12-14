using CourseWorkMailClient.Domain;
using Lab6;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CourseWorkMailClient.Infrastructure
{
    public static class PrepareData
    {
        public static string GetParsedFolderName(string folderName)
        {
            return folderName == "INBOX" ? "Входящие" : folderName;
        }

        public static int? GetFolderTypeId(FolderAttributes attributes)
        {
            if (attributes.HasFlag(FolderAttributes.Inbox))
                return 1;
            if (attributes.HasFlag(FolderAttributes.Sent))
                return 2;
            if (attributes.HasFlag(FolderAttributes.Trash))
                return 3;
            if (attributes.HasFlag(FolderAttributes.Drafts))
                return 4;

            return null;
        }

        public static string ContentToHTML(IEnumerable<LightParagraph> lightParagraphs)
        {
            var brushesConverter = new BrushConverter();

            var htmlTemplete = "<!DOCTYPE html ><html><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'><head></head><body> {0} </body></html>";
            var paragraphTemplete = "<p>{0}</p>";
            var divTemplete = "<div style=\"{0}\">{1}</div>";

            var content = "";
            foreach (var lightParagraph in lightParagraphs)
            {
                var divContent = "";
                foreach (var run in lightParagraph.Inlines)
                {
                    var color = run.Foreground != null ? string.Format("color: {0};", "#" + run.Foreground.ToString()[3..]) : "";
                    var background = run.Background != null ? string.Format("background: {0};", "#" + run.Background.ToString()[3..]) : "";
                    var fontSize = string.Format("font-size: {0}pt;", run.FontSize.ToString());
                    var fontWeight = string.Format("font-weight: {0};", run.FontWeight.ToString());
                    var fontStyle = string.Format("font-style: {0};", run.FontStyle.ToString());
                    var textDecorations = run.TextDecorations.Count != 0 ? string.Format("text-decoration: {0};", "underline") : "";

                    divContent += string.Format(divTemplete, color + background + fontSize + fontWeight + fontStyle + textDecorations, run.Text);
                }
                content += string.Format(paragraphTemplete, divContent);
            }

            return string.Format(htmlTemplete, content);
        }

        public static Letter ExtractKeysFromServerMes(Letter message)
        {
            if (message.Source.Headers.Contains(HeaderId.Encrypted))
            {
                var sender = message.Senders.First();
                sender = HandlerService.Repository.GetInterlocutor(sender.Email, true);
                //var receiver = HandlerService.Repository.GetInterlocutor(message.Receivers.First().Email, true);
                //Если изменился публичный ключ отправителя, то поменять последний у отправителя
                var letterDESKey = message.Source.Headers.First(h => h.Id == HeaderId.Encrypted)?.Value;
                if (sender.LastDESRsaKey?.PublicKey != letterDESKey)
                {
                    var des = new CryptoDES();
                    des.CreateNewRsaKey();
                    des.SetRsaKey(letterDESKey);
                    sender.LastDESRsaKey = des.GetRsaKey();
                }

                var letterMD5Key = message.Source.Headers.First(h => h.Id == HeaderId.Summary)?.Value;
                if (sender.LastMD5RsaKey?.PublicKey != letterMD5Key)
                {
                    var md5 = new CryptoMD5();
                    md5.CreateNewRsaKey();
                    md5.SetRsaKey(letterMD5Key);
                    sender.LastMD5RsaKey = md5.GetRsaKey();
                }

                message.MD5RsaKey = sender.LastMD5RsaKey;
                message.DESRsaKey = sender.UserLastDESRsaKey;
            }

            return message;
        }

        public static void CreateUsersKeys(Interlocutor interlocutor)
        {
            if (interlocutor.UserLastDESRsaKeyId == null || interlocutor.UserLastMD5RsaKeyId == null)
            {
                var md5Sender = new CryptoMD5();
                var desSender = new CryptoDES();

                md5Sender.CreateNewRsaKey();
                desSender.CreateNewRsaKey();

                interlocutor.UserLastMD5RsaKey = md5Sender.GetRsaKey();
                interlocutor.UserLastDESRsaKey = desSender.GetRsaKey();
            }
        }

        public static MimeMessage PrepareLetterForSent(Letter letter)
        {
            var message = new MimeMessage();

            var user = GetDataService.ActualUser;

            message.From.Add(new MailboxAddress(user.Login, user.Login));

            letter.Receivers.ForEach(h => message.To.Add(new MailboxAddress(h.Email, h.Email)));

            letter.Senders = new List<Interlocutor>();
            letter.Senders.Add(HandlerService.Repository.GetOrCreateInterlocutor(user.Login));

            message.Subject = letter.Subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = letter.Content;

            letter.Attachments?.ForEach(h => builder.Attachments.Add(h.Name));

            message.Body = builder.ToMessageBody();

            return message;
        }
    }
}
