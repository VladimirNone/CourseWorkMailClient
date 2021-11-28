using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Infrastructure;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            HandlerService.ActualMessagesChanged += (messages) =>
            {
                lbMesList.ItemsSource = null;
                lbMesList.ItemsSource = messages;
            };
        }

        private void bWriteMes_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WriteLetterPage(this));
        }

        private void bReadMes_Click(object sender, MouseButtonEventArgs e)
        {
            var mes = (LightMessage)((ListBoxItem)sender).DataContext;

            NavigationService.Navigate(new ReadLetterPage(this, mes));
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if(lbMesList.SelectedItem == null)
            {
                miMoveToOtherFolder.IsEnabled = false;
                miDelete.IsEnabled = false;
            }
            else
            {
                miMoveToOtherFolder.IsEnabled = true;
                miDelete.IsEnabled = true;
            }
        }

        private void bExit_Click(object sender, RoutedEventArgs e)
        {
            HandlerService.UnAuth();
            NavigationService.Navigate(new AuthPage());
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miMoveToOtherFolder_Click(object sender, RoutedEventArgs e)
        {
            var mes = (LightMessage)((ListBoxItem)sender).DataContext;
            //HandlerService.KitImapHandler.MoveMessage(mes, HandlerService.KitImapHandler.getf)
        }
        
    }
}
