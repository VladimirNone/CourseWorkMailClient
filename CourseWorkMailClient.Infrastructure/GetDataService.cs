using CourseWorkMailClient.Domain;
using MailKit;
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
                if(actualFolder != null && actualFolder != value && actualFolder.Source != null)
                    HandlerService.KitImapHandler.CloseFolder(actualFolder);

                actualFolder = value;
                Pagination.Page = 1;
                Pagination.ChangePage(0);
                if(ChangeActualFolder != null)
                    ChangeActualFolder();
            }
        }

        public static Action ChangeActualFolder;

        public static MailServer ActualMailServer { get; set; }

        public static PaginationService Pagination { get; set; } = new PaginationService();

        public static List<UniqueId> uniqueIdsLastFolder { get; set; }
        public static List<UniqueId> uniqueIdsCurrentPage { get; set; }

        static GetDataService()
        {
            Pagination.ChangingPage += (curPage, itemsOnPage) =>
            {
                uniqueIdsCurrentPage = uniqueIdsLastFolder.Skip((curPage - 1) * itemsOnPage).Take(itemsOnPage).ToList();

                Letters.Reset(GetMessages(ActualFolder));
            };
        }

        public static void OpenFolder(Folder folder)
        {
            if(folder.Source != null)
            {
                HandlerService.KitImapHandler.OpenFolder(folder);
            }
            else
            {
                folder.CountOfMessage = HandlerService.repo.GetCountOfMessages(folder.Id);
                var uids = HandlerService.repo.GetUniqueIds(folder);
                uniqueIdsLastFolder = uids.Select(h => new UniqueId((uint)h)).ToList();
                Pagination.MaxCountOfPage = (int)folder.CountOfMessage / Pagination.ItemsOnPage + ((int)folder.CountOfMessage % Pagination.ItemsOnPage == 0 ? 0 : 1);
            }
        }

        public static List<Folder> GetFolders()
        {
            var folders = new List<Folder>();
            if (HandlerService.KitImapHandler == null || !HandlerService.KitImapHandler.IsConnected)
            {
                folders = HandlerService.repo.GetFolders(ActualMailServer);
            }
            else
            {
                folders = HandlerService.KitImapHandler.GetServerFolders();

                HandlerService.repo.SelectAndAddNewFolders(folders, ActualMailServer);
                HandlerService.repo.SaveChanged();

                var dbFolders = HandlerService.repo.GetFolders(ActualMailServer);

                foreach (var item in dbFolders)
                {
                    item.Source = folders.First(h => h.Title == item.Title).Source;
                }

                folders = dbFolders;
            }

            return folders;
        }

        /// <summary>
        /// Возвращает полную версию сообщения
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static Letter GetMessage(Letter Message, Folder folder)
        {
            var mesFromDb = HandlerService.repo.GetMessage(Message.UniqueId, lightVersion: false);

            //в теории никогда не будет использоваться
            if(mesFromDb == null)
            {
                mesFromDb = HandlerService.KitImapHandler.GetFullMessage(Message, folder);
                HandlerService.repo.AddMessage(mesFromDb);
                HandlerService.repo.SaveChanged();
            }

            if (mesFromDb.PathToFullMessageFile != null)
            {
                mesFromDb.Source = HandlerService.KitImapHandler.GetMimeMessage(mesFromDb.PathToFullMessageFile);
            }
            else
            {
                mesFromDb = HandlerService.KitImapHandler.GetFullMessage(mesFromDb, folder);
                HandlerService.repo.SaveChanged();
            }

            return mesFromDb;
        }

        public static List<Letter> GetMessages(Folder folder)
        {
            List<Letter> messages = null;
            if (folder.Source == null)
            {
                messages = HandlerService.repo.GetMessages(folder.Id, uniqueIdsCurrentPage.Select(h => (int)h.Id).ToList());
            }
            else
            {
                messages = HandlerService.KitImapHandler.GetMessages(folder);
                messages = HandlerService.repo.SelectAndAddNewLetters(messages, folder);
                HandlerService.repo.SaveChanged();
            }

            return messages;
        }
    }
}
