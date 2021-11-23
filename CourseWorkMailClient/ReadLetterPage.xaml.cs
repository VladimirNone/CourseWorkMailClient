using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Infrastructure;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.IO;
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
        private uint MessageId;

        public ReadLetterPage(Page previousPage, CustomMessage messageForRead)
        {
            InitializeComponent();

            prevPage = previousPage;

            //Костыль. Необходимо определить кодировку.
            var content = @"<!DOCTYPE html ><html><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'><head></head><body>" + messageForRead.Content + "</body></html>";

            MessageId = messageForRead.Id;
            tbReceivers.Text = string.Join(", ", messageForRead.To);
            tbSubject.Text = messageForRead.Subject;
            tbDate.Text += messageForRead.Date;
            wbContent.NavigateToString(content);

            lbAttachments.ItemsSource = messageForRead.Attachments;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(prevPage);
        }

        private void ButtonDownloadAll_Click(object sender, RoutedEventArgs e)
        {
            Handlers.KitImapHandler.DownloadAttachments((List<string>)lbAttachments.ItemsSource, "", MessageId);
        }

        private void ButtonDownloadOne_Click(object sender, RoutedEventArgs e)
        {
            var fileName = (string)((ListBoxItem)lbAttachments.SelectedItem).Content;
            Handlers.KitImapHandler.DownloadAttachment(fileName, "", MessageId);
        }
    }
}
