using System.Linq;
using CourseWorkMailClient.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;
using System.Windows;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MimeKit;

namespace CourseWorkMailClient.Infrastructure
{
    public static class ParallelFunction
    {
        public static void SyncFolders(FrameworkElement caller)
        {
            Task.Run(() =>
            {
                if (HandlerService.KitImapHandler == null)
                {
                    return;
                }

                var folders = HandlerService.KitImapHandler.GetServerFolders();

                HandlerService.repo.SelectAndAddNewFolders(folders, GetDataService.ActualMailServer);
                HandlerService.repo.SaveChanged();

                var dbFolders = HandlerService.repo.GetFolders(GetDataService.ActualMailServer);

                foreach (var item in dbFolders)
                {
                    item.Source = folders.First(h => h.Title == item.Title).Source;
                }

                caller.Dispatcher.Invoke(() =>
                {
                    GetDataService.Folders.Reset(dbFolders);

                    if (GetDataService.ActualFolder != null && dbFolders.Find(h => h.Title == GetDataService.ActualFolder.Title) == null)
                    {
                        GetDataService.ActualFolder = dbFolders.First();
                    }
                });
            });
        }

        public static void LoadLastLetter(Folder folder, int count = 50)
        {
            var uids = folder.Source.Search(SearchQuery.All).Take(count).ToList();
            var lastLetters = folder.Source.Fetch(uids, MessageSummaryItems.Headers);
            var letters = new List<Letter>();

            foreach (var item in lastLetters)
            {
                var newLetter = HandlerService.mapper.Map<MimeMessage, Letter>(new MimeMessage(item.Headers));
                newLetter.Folders.Add(folder);
                letters.Add(newLetter);
            }


        }

        public static void SyncFolderMessages(FrameworkElement caller, Folder folder)
        {
            Task.Run(() =>
            {
                if (HandlerService.KitImapHandler == null)
                {
                    return;
                }


            });
        }
    }
}
