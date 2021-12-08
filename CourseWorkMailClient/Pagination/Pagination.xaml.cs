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

namespace CourseWorkMailClient.Pagination
{
    /// <summary>
    /// Interaction logic for Pagination.xaml
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();

            GetDataService.Pagination.ChangingPage += (page, countItems) =>
            {
                tbPagination.Text = page + "/" + GetDataService.Pagination.MaxCountOfPage;
            };
        }

        private void bChangePageForward_Click(object sender, RoutedEventArgs e)
            => GetDataService.Pagination.ChangePage(1);

        private void bChangePageBack_Click(object sender, RoutedEventArgs e)
            => GetDataService.Pagination.ChangePage(-1);

        private void bChangePagesForward_Click(object sender, RoutedEventArgs e)
            => GetDataService.Pagination.ChangePage(10);

        private void bChangePagesBack_Click(object sender, RoutedEventArgs e)
            => GetDataService.Pagination.ChangePage(-10);

        private void bChangePagesToStart_Click(object sender, RoutedEventArgs e)
            => GetDataService.Pagination.ChangePage(1 - GetDataService.Pagination.Page);

        private void bChangePagesToEnd_Click(object sender, RoutedEventArgs e)
            => GetDataService.Pagination.ChangePage(GetDataService.Pagination.MaxCountOfPage - GetDataService.Pagination.Page);
    }
}
