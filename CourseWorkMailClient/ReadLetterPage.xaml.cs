﻿using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Infrastructure;
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
        private LightMessage Message;

        public ReadLetterPage(Page previousPage, LightMessage messageForRead)
        {
            InitializeComponent();

            prevPage = previousPage;

            //Костыль. Необходимо определить кодировку.
            var content = @"<!DOCTYPE html ><html><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'><head></head><body>" + messageForRead.Content + "</body></html>";

            Message = messageForRead;
            tbReceivers.Text = string.Join(", ", messageForRead.To);
            tbSubject.Text = messageForRead.Subject;
            tbDate.Text += messageForRead.Date;

            if(messageForRead.LocalMessage)
            {
                rtbContent.Visibility = Visibility.Visible;
                wbContent.Visibility = Visibility.Hidden;

                rtbContent.Document.Blocks.Clear();

                var lightParagraphs = JsonConvert.DeserializeObject<LightParagraph[]>(messageForRead.Content);

                foreach (var lightParagraph in lightParagraphs)
                {
                    var paragraph = new Paragraph();
                    paragraph.Inlines.AddRange(lightParagraph.Inlines.Select(h => HandlerService.mapper.Map<Run>(h)));

                    rtbContent.Document.Blocks.Add(paragraph);
                }
            }
            else
            {
                rtbContent.Visibility = Visibility.Hidden;
                wbContent.Visibility = Visibility.Visible;

                wbContent.NavigateToString(content);
            }

            lbAttachments.ItemsSource = messageForRead.Attachments;
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
            var fileName = (string)((ListBoxItem)lbAttachments.SelectedItem).Content;
            HandlerService.KitImapHandler.DownloadAttachment(fileName, "", Message.Source);
        }
    }
}
