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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseWorkMailClient
{
    /// <summary>
    /// Interaction logic for ReadLetterPage.xaml
    /// </summary>
    public partial class ReadLetterPage : Page
    {
        private Page prevPage;

        public ReadLetterPage(Page previousPage)
        {
            prevPage = previousPage;
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(prevPage);
        }

        private void ButtonDownloadAll_Click(object sender, RoutedEventArgs e)
        {
            //MainPage.receiver.DownloadAttachments(Message.Attachments, "", Message.Id);
        }

        private void ButtonDownloadOne_Click(object sender, RoutedEventArgs e)
        {
/*            var fileName = (string)((ListBoxItem)lbAttachments.SelectedItem).Content;
            MainPage.receiver.DownloadAttachment(fileName, "", Message.Id);*/
        }
    }
}
