using CourseWorkMailClient.LoadLetters;
using CourseWorkMailClient.Infrastructure;
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
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();

            var users = HandlerService.repo.GetUsers();

            cbUsers.ItemsSource = users;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(tbPassword.Text) || !string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                HandlerService.Auth(tbLogin.Text, tbPassword.Text);

                var authingWindow = new LoadingWindow();
                authingWindow.Load();
                authingWindow.ShowDialog();
            }
            else if(cbUsers.SelectedItem != null)
            {
                HandlerService.Auth((Domain.User)cbUsers.SelectedItem);

                var authingWindow = new LoadingWindow();
                authingWindow.Load();
                authingWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Вы не ввели достаточно данных, необходимых для авторизации");
                return;
            }

            NavigationService.Navigate(new MainPage());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void cbUsers_Selected(object sender, RoutedEventArgs e)
        {
            tbLogin.Text = "";
            tbPassword.Text = "";
        }
    }
}
