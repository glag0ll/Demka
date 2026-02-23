using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Models;
using WpfApp1.Views.Controls;

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для productListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        public ProductListPage()
        {
            InitializeComponent();
            loadProducts();
        }

        public void loadProducts()
        {
            var context = new ApplicationDbContext();
            var products = context.Products.Include(p => p.Category).Include(p => p.Supplier).Include(p => p.Manufacturer).ToList();

            productList.Items.Clear();
            foreach ( var product in products )
            {
                var item = new productControl();
                item.SetData(product);
                productList.Items.Add(item);
            }
        }

        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productControl selectedProdct = (productControl)productList.SelectedItem;
            int productId = selectedProdct.productId;
            NavigationService.Navigate(new productDataPage(productId));
        }
    }
}
