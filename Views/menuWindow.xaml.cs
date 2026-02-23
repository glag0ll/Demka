using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.Views
{
    public partial class menuWindow : Window
    {
        public menuWindow()
        {
            InitializeComponent();
        }

        private void navProductList_Click(object sender, RoutedEventArgs e)
        {
            navMain.Source = new Uri ("/Views/Pages/ProductListPage.xaml", UriKind.Relative);
        }

        private void navOrderList_Click(object sender, RoutedEventArgs e)
        {
            navMain.Source = new Uri("/Views/Pages/ordersListPage.xaml", UriKind.Relative);
        }

        private void navNewProduct_click(object sender, RoutedEventArgs e)
        {
            navMain.Source = new Uri("/Views/Pages/productDataPage.xaml", UriKind.Relative);
        }
    }
}
