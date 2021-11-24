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
        private CustomNotifyCollectionCollection<CustomFolder> folders;
        public NavigateMenu()
        {
            InitializeComponent();

            folders = new CustomNotifyCollectionCollection<CustomFolder>(Handlers.KitImapHandler.GetFolders());

            lbNavMenu.ItemsSource = folders;
        }

        public void OpenFolder(object sender, RoutedEventArgs e)
        {
            var folder = (CustomFolder)((ListBoxItem)sender).DataContext;

            //Костыль. Необходимо получить копию объекта folder
            var copyFolder = Handlers.KitImapHandler.GetFolder(folder.Source);

            copyFolder.Source.Open(FolderAccess.ReadWrite);
            copyFolder.CountOfMessage = copyFolder.Source.Count;

            folders.Replace(folder, copyFolder);

            Handlers.ActualMessages = Handlers.KitImapHandler.GetMessages(copyFolder.Source);
        }

        public void CloseFolder(object sender, RoutedEventArgs e)
        {
            var folder = (CustomFolder)((ListBoxItem)sender).DataContext;
            folder.Source.Close();
        }
    }
}
