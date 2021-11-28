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

namespace CourseWorkMailClient.FolderItems
{
    /// <summary>
    /// Interaction logic for CreateFolderWindow.xaml
    /// </summary>
    public partial class CreateFolderWindow : Window
    {
        public string NameNewFolder
        {
            get => tbNameFolder.Text;
        }

        public MailFolder ParentFolder
        {
            get => cbFolderNames.SelectedItem == null ? null : (MailFolder)((ComboBoxItem)cbFolderNames.SelectedItem).Tag;
        }

        public CreateFolderWindow(List<(string, MailFolder)> foldersWithId)
        {
            InitializeComponent();

            foreach (var item in foldersWithId)
            {
                cbFolderNames.Items.Add(new ComboBoxItem() { Content = item.Item1, Tag = item.Item2 });
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
