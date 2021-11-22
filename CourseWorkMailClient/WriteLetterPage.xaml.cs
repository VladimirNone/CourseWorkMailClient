using Microsoft.Win32;
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
    /// Interaction logic for WriteLetterPage.xaml
    /// </summary>
    public partial class WriteLetterPage : Page
    {
        private Page prevPage;

        public WriteLetterPage(Page previousPage)
        {
            prevPage = previousPage;
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(prevPage);
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(prevPage);
        }

        private void ButtonAddAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonRemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
