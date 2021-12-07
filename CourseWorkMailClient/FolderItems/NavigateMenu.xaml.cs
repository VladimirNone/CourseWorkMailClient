using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Infrastructure;
using MailKit;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWorkMailClient.FolderItems
{
    /// <summary>
    /// Interaction logic for NavigateMenu.xaml
    /// </summary>
    public partial class NavigateMenu : UserControl
    {
        public NavigateMenu()
        {
            GetDataService.navigatorControl = this;

            InitializeComponent();

            GetDataService.Folders = new CustomNotifyCollectionCollection<Folder>(GetDataService.GetFolders());

            lbNavMenu.ItemsSource = GetDataService.Folders;
        }

        public void OpenFolder(object sender, RoutedEventArgs e)
        {
            var folder = (Folder)((ListBoxItem)sender).DataContext;

            HandlerService.KitImapHandler.OpenFolder(folder);

            GetDataService.ActualFolder = folder;
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (HandlerService.KitImapHandler == null)
            {
                miCreate.IsEnabled = false;
                miDelete.IsEnabled = false;
                miRename.IsEnabled = false;
                return;
            }

            if (lbNavMenu.SelectedItem == null)
            {
                miDelete.IsEnabled = false;
                miRename.IsEnabled = false;
            }
            else
            {
                miDelete.IsEnabled = true;
                miRename.IsEnabled = true;
            }
        }

        public void CloseFolder(object sender, RoutedEventArgs e)
        {
            var folder = (Folder)((ListBoxItem)sender).DataContext;

            HandlerService.KitImapHandler.CloseFolder(folder);
        }

        private void miCreate_Click(object sender, RoutedEventArgs e)
        {
            var createFolderWindow = new CreateFolderWindow(GetDataService.Folders.Select(h=> (h.Title, h.Source)).ToList());

            if (createFolderWindow.ShowDialog() == true)
            {
                GetDataService.Folders.Add(HandlerService.KitImapHandler.CreateNewFolder(createFolderWindow.NameNewFolder, createFolderWindow.ParentFolder));
            }
        }

        private void miRename_Click(object sender, RoutedEventArgs e)
        {
            var folder = (Folder)lbNavMenu.SelectedItem;
            var renameFolderWindow = new RenameFolderWindow();

            if (renameFolderWindow.ShowDialog() == true)
            {
                var updatedFolder = HandlerService.KitImapHandler.RenameFolder(renameFolderWindow.NewFolderName, folder.Source);
                updatedFolder.CountOfMessage = folder.CountOfMessage;
                GetDataService.Folders.Replace(folder, updatedFolder);
                lbNavMenu.SelectedItem = updatedFolder;
            }
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            var folder = (Folder)lbNavMenu.SelectedItem;
            HandlerService.KitImapHandler.DeleteFolder(folder.Source);
            GetDataService.Folders.Delete(folder);
        }
    }
}
