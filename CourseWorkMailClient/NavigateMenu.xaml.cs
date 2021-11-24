using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Infrastructure;
using MailKit;
using System;
using System.Collections.Generic;
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
        public NavigateMenu()
        {
            InitializeComponent();

            var folders = Handlers.KitImapHandler.GetFolders();

            lbNavMenu.ItemsSource = folders;
        }

        public void OpenFolder(object sender, RoutedEventArgs e)
        {
            var folder = (CustomFolder)((ListBoxItem)sender).DataContext;
            folder.Source.Open(FolderAccess.ReadWrite);
            Handlers.ActualMessages = Handlers.KitImapHandler.GetMessages(folder.Source);
        }

        public void CloseFolder(object sender, RoutedEventArgs e)
        {
            var folder = (CustomFolder)((ListBoxItem)sender).DataContext;
            folder.Source.Close();
        }
    }
}
