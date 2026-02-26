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

namespace WpfApp1.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для productControl.xaml
    /// </summary>
    public partial class productControl : UserControl
    {
        public int productId;

        public productControl()
        {
            InitializeComponent();
        }

        public void SetData(Product product) 
        {
            productId = product.Id;

            var finalPrice = product.Price * (1 - product.Discount / 100);

            txtId.Text = product.Id.ToString();
            txtName.Text = $"{product.Category.Name} | {product.Name}";
            txtDescription.Text = $"Описание товара: {product.Description}";
            txtManufactirer.Text = $"Производитель: {product.Manufacturer.Name}";
            txtSupplier.Text = $"Поставщик: {product.Supplier.Name}";
            txtPrice.Text = $"{product.Price}"; txtFinalPrice.Text = $" {finalPrice}";
            txtUnit.Text = $"Скидка: {product.Unit}";
            txtQuantity.Text = $"Количество на складе: {product.StockQuantity}";

            if (!string.IsNullOrEmpty(product.Photo))
            {
                imgPhoto.Source = new BitmapImage(new Uri(
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shopResources", product.Photo), UriKind.Absolute));
            }
            else 
            {
                imgPhoto.Source = new BitmapImage(new Uri(
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shopResources", "picture.png"), UriKind.Absolute));
            }
            txtDiscount.Text = $"{product.Discount}%";

            if (product.Discount > 15)
            {
                string hex = "#2E8B57";
                Color color = (Color)ColorConverter.ConvertFromString(hex);
                txtDiscount.Foreground = new SolidColorBrush(color);
            }
            else if (product.Discount == 0)
            {
                txtPrice.Text = "";
            }
            if (product.StockQuantity == 0)
            {
                txtQuantity.Foreground = Brushes.LightBlue;
            }
        }
    }
}
