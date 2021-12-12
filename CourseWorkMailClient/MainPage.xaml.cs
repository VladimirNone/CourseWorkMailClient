﻿using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Infrastructure;
using CourseWorkMailClient.LoadLetters;
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

            lbMesList.ItemsSource = GetDataService.Letters;
        }
        
        private void bbUpdateMesList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bWriteMes_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WriteLetterPage(this));
        }

        private void bReadMes_Click(object sender, MouseButtonEventArgs e)
        {
            var mes = (Letter)((ListBoxItem)sender).DataContext;

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
                miMoveToOtherFolder.ItemsSource = GetDataService.GetMovableFolders();
                miMoveToOtherFolder.IsEnabled = true;
                miDelete.IsEnabled = true;
            }
        }
        
        private void bSaveLettersFromPage_Click(object sender, RoutedEventArgs e)
        {
            var authingWindow = new LoadingWindow("Происходит загрузка писем");
            authingWindow.Load((folder) => HandlerService.KitImapHandler.LoadLastLetters((Folder)folder), GetDataService.ActualFolder);
            authingWindow.ShowDialog();
            HandlerService.Repository.SaveChanged();
        }

        private void bExit_Click(object sender, RoutedEventArgs e)
        {
            HandlerService.UnAuth();
            NavigationService.Navigate(new AuthPage());
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            _ = 5;
        }

        private void miMoveToOtherFolder_Click(object sender, RoutedEventArgs e)
        {
            var mes = (Letter)((ListBoxItem)sender).DataContext;

            //HandlerService.KitImapHandler.MoveMessage(mes, HandlerService.KitImapHandler.getf)
        }
        
    }
}
