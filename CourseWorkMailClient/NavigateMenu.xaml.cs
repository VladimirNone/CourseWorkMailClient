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

            lbNavMenu.ItemsSource = new List<object>
            {
                new {Title = "Входящие", CountOfMessage=15}
            };
        }

        private void ChangeFolder_Click(object sender, RoutedEventArgs e)
        {
            Handlers.ActualMessages = Handlers.KitImapHandler.GetMessages();
        }
    }
}
