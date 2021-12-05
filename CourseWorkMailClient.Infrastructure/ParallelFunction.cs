using System.Linq;
using CourseWorkMailClient.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;
using System.Windows;

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
                    folders.First(h => h.Title == item.Title).Id = item.Id;
                }

                caller.Dispatcher.Invoke(() =>
                {
                    GetDataService.Folders.Reset(folders);

                    if (GetDataService.ActualFolder != null && folders.Find(h => h.Title == GetDataService.ActualFolder.Title) == null)
                    {
                        GetDataService.ActualFolder = folders.First();
                    }
                });
            });
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
