using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Infrastructure;
using Lab6;
using Newtonsoft.Json;
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
        private Letter Message;

        public ReadLetterPage(Page previousPage, Letter messageForRead)
        {
            GetDataService.ChangeActualFolder += () =>
            {
                if(NavigationService != null)
                    NavigationService.Navigate(previousPage);
            };

            prevPage = previousPage;

            InitializeComponent();

            Message = GetDataService.GetMessage(messageForRead, GetDataService.ActualFolder);

            //Костыль. Необходимо определить кодировку.
            var content = @"<!DOCTYPE html ><html><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'><head></head><body>" + Message.Content + "</body></html>";

            tbReceivers.Text = Message.To;
            tbSenders.Text = Message.From;
            tbSubject.Text = Message.Subject;
            tbDate.Text += Message.Date;

            wbContent.NavigateToString(content);

            lbAttachments.ItemsSource = Message.Attachments.Select(h=>h.Name);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(prevPage);
        }

        private void ButtonDownloadAll_Click(object sender, RoutedEventArgs e)
        {
            HandlerService.KitImapHandler.DownloadAttachments((List<string>)lbAttachments.ItemsSource, "", Message.Source);
        }

        private void ButtonDownloadOne_Click(object sender, RoutedEventArgs e)
        {
            var fileName = (string)lbAttachments.SelectedItem;
            HandlerService.KitImapHandler.DownloadAttachment(fileName, "", Message.Source);
        }
    }
}
