using CourseWorkMailClient.Domain;
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

namespace CourseWorkMailClient.LoadLetters
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow(string content)
        {
            InitializeComponent();

            tbTitle.Text = content;
        }

        public void Load(Action<string, string> action, string login, string password)
        {
            Task.Run(() =>
            {
                action(login, password);
                Dispatcher.Invoke(() =>
                {
                    DialogResult = true;
                });
                
            });

        }

        public void Load(Action<object> action, object obj)
        {
            Task.Run(() =>
            {
                action(obj);
                Dispatcher.Invoke(() =>
                {
                    DialogResult = true;
                });

            });

        }
    }
}
