using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class SupplierOption
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name.ToString();
        }
    }
    public partial class ProductListPage : Page
    {
        public ProductListPage()
        {
            InitializeComponent();
            loadSuppliers();
        }
        
        public void FilterChanged(object sender, EventArgs e)
        {
            applyFilters();
        }

        public void applyFilters()
        {
            var search = (SearchBox.Text ?? "").ToLower().Trim();
            SupplierOption selected = SupplierCombo.SelectedItem as SupplierOption;
            bool sortAscBool = sortAsc.IsChecked == true;
            int supplierId = selected?.ID ?? 0;
            var context = new ApplicationDbContext();
            var products = context.Products.Include(p => p.Category).Include(p => p.Supplier).Include(p => p.Manufacturer).ToList();
            var query = products.AsQueryable();

            if (search.Length > 0)
            {
                query = query.Where(x => x.Article.ToLower().Contains(search) || x.Name.ToLower().Contains(search) || 
                    x.Description.ToLower().Contains(search) || x.Unit.ToLower().Contains(search) || x.Price.ToString().ToLower().Contains(search) || x.Discount.ToString().ToLower().Contains(search) || x.StockQuantity.ToString().ToLower().Contains(search));
            }

            if (supplierId != 0)
            {
                query = query.Where(x => x.SupplierId == supplierId);
            }

            query = sortAscBool
                ? query.OrderBy(x => x.StockQuantity)
                : query.OrderByDescending(x => x.StockQuantity);

            loadProducts(query.ToList());
        }

        public void loadSuppliers()
        {
            SupplierCombo.Items.Add(new SupplierOption { ID = 0, Name = "Все поставщики"});
            var context = new ApplicationDbContext();
            var supplers = context.Suppliers.OrderBy(s => s.Name).Select(s => new SupplierOption { ID = s.Id, Name = s.Name  }).ToList();
            foreach (var s in supplers)       
            {
                SupplierCombo.Items.Add(s);
            }
            SupplierCombo.SelectedIndex = 0;

            applyFilters();
        }

        public void loadProducts(List<Product> products)
        {
            if (productList == null) return; 

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
