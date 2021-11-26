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

namespace CourseWorkMailClient
{
    /// <summary>
    /// Interaction logic for NavigateMenu.xaml
    /// </summary>
    public partial class NavigateMenu : UserControl
    {
        private CustomNotifyCollectionCollection<LightFolder> folders;
        public NavigateMenu()
        {
            InitializeComponent();

            folders = new CustomNotifyCollectionCollection<LightFolder>(HandlerService.KitImapHandler.GetFolders());

            lbNavMenu.ItemsSource = folders;
        }

        public void OpenFolder(object sender, RoutedEventArgs e)
        {
            var folder = (LightFolder)((ListBoxItem)sender).DataContext;

            //Костыль. Необходимо получить копию объекта folder
            var copyFolder = HandlerService.KitImapHandler.GetFolder(folder.Source);

            copyFolder.Source.Open(FolderAccess.ReadWrite);
            copyFolder.CountOfMessage = copyFolder.Source.Count;

            folders.Replace(folder, copyFolder);

            HandlerService.ActualMessages = HandlerService.KitImapHandler.GetMessages(copyFolder.Source);
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (lbNavMenu.SelectedItem == null)
            {
                miDelete.IsEnabled = false;
            }
            else
            {
                miDelete.IsEnabled = true;
            }
        }

        public void CloseFolder(object sender, RoutedEventArgs e)
        {
            var folder = (LightFolder)((ListBoxItem)sender).DataContext;
            folder.Source.Close();
        }

        private void miCreate_Click(object sender, RoutedEventArgs e)
        {
            var createFolderWindow = new CreateFolderWindow(folders.Select(h=> (h.Title, h.Source)).ToList());

            if (createFolderWindow.ShowDialog() == true)
            {
                HandlerService.KitImapHandler.CreateNewFolder(createFolderWindow.NameNewFolder, createFolderWindow.ParentFolder);
            }
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
