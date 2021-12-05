using CourseWorkMailClient.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CourseWorkMailClient.Infrastructure
{
    public static class GetDataService
    {
        public static FrameworkElement mainPage { get; set; }
        public static FrameworkElement navigatorControl { get; set; }

        public static CustomNotifyCollectionCollection<Letter> Letters { get; set; } = new CustomNotifyCollectionCollection<Letter>();

        public static CustomNotifyCollectionCollection<Folder> Folders;
        private static Folder actualFolder;
        public static Folder ActualFolder
        {
            get => actualFolder;
            set
            {
                actualFolder = value;
                Letters.Reset(GetMessages(value));
            }
        }

        public static MailServer ActualMailServer { get; set; }

        public static List<Folder> GetFolders()
        {
            var foldersFromDb = HandlerService.repo.GetFolders(ActualMailServer);

            //подгружает папки с сервера
            ParallelFunction.SyncFolders(navigatorControl);
            
            return foldersFromDb;
        }

        /// <summary>
        /// Возвращает полную версию сообщения
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static Letter GetMessage(Letter Message, Folder folder)
        {
            var mesFromDb = HandlerService.repo.GetMessage(Message.MessageId, lightVersion: false);
            if (mesFromDb != null && mesFromDb.PathToFullMessageFile != null)
            {
                mesFromDb.Source = HandlerService.KitImapHandler.GetMimeMessage(mesFromDb.PathToFullMessageFile);

                return mesFromDb;
            }
            else
            {
                //удаляю этот объект, т.к. mesFromDb не отслеживается
                HandlerService.repo.RemoveMessage(mesFromDb.Id);
                HandlerService.repo.SaveChanged();
                var mesFromServer = HandlerService.KitImapHandler.GetMessage(Message.MessageId, folder);
                HandlerService.repo.AddMessage(mesFromServer);
                HandlerService.repo.SaveChanged();

                return mesFromServer;
            }
        }

        public static List<Letter> GetMessages(Folder folder)
        {
            List<Letter> messages = null;
            if (folder.Source == null)
            {
                messages = HandlerService.repo.GetMessages(folder.Id);
            }
            else
            {
                messages = HandlerService.KitImapHandler.GetMessages(folder);
                HandlerService.repo.SelectAndAddNewLetters(messages, folder);
                HandlerService.repo.SaveChanged();
            }

            return messages;
        }
    }
}
