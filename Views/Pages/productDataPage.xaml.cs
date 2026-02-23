using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1.Views.Pages
{
    public partial class productDataPage : Page
    {
        private string filePath;
        private int selectedProductId;
        public productDataPage(int? productId = null)
        {
            InitializeComponent();
            loadData();
            if (productId != null )
            {
                selectedProductId = (int)productId;
                loadProduct(selectedProductId);
                buttonDelete.Visibility = Visibility.Visible;
            }
        }

        public void loadProduct(int productId)
        {
            var context = new ApplicationDbContext();
            Product product = context.Products.Where(p => p.Id == productId).FirstOrDefault();

            productName.Text = product.Name;
            productDescription.Text = product.Description;
            productArticle.Text = product.Article;
            productPrice.Value = (double)product.Price;
            productDiscount.Value = (double)product.Discount;
            productQuantity.Value = (int)product.StockQuantity;
            productUnit.Text = product.Unit;
            productCategory.SelectedIndex = product.CategoryId - 1;
            productManufacturer.SelectedIndex = product.ManufacturerId - 1;
            productSupplier.SelectedIndex = product.SupplierId - 1;

            filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shopResources", product.Photo == null ? "picture.png" : product.Photo);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            productImage.Source = bitmap;
        }

        public string saveImage(string imagePath)
        {
            var fileInfo = new FileInfo(imagePath);
            var destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shopResources", fileInfo.Name);
            File.Copy(filePath, destinationPath, overwrite:true);

            return fileInfo.Name;
        }

        public void loadData()
        {
            var context = new ApplicationDbContext();
            var categories = context.Categories.ToList();
            var manufacturers = context.Manufacturers.ToList();
            var suppliers = context.Suppliers.ToList();

            productCategory.ItemsSource =  categories;
            productCategory.SelectedIndex = 0;
            productManufacturer.ItemsSource = manufacturers;
            productManufacturer.SelectedIndex = 0;
            productSupplier.ItemsSource = suppliers;
            productSupplier.SelectedIndex = 0;
        }


        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(productName.Text) || string.IsNullOrWhiteSpace(productArticle.Text) || string.IsNullOrEmpty(productDescription.Text) ||
                string.IsNullOrWhiteSpace(productPrice.Text) || string.IsNullOrWhiteSpace(productDiscount.Text) || string.IsNullOrWhiteSpace(productQuantity.Text) ||
                string.IsNullOrWhiteSpace(productUnit.Text)) 
            {
                MessageBox.Show("Заполните все поля для продолжения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string imagePath = null;
            if (!string.IsNullOrWhiteSpace(productImage.Source.ToString()))
            {
                imagePath = saveImage(filePath);
            }

            var product = new Product
            {
                Name = productName.Text.Trim(),
                Article = productArticle.Text.Trim(),
                Description = productDescription.Text.Trim(),
                Price = decimal.Parse(productPrice.Text),
                Discount = decimal.Parse(productDiscount.Text),
                StockQuantity = int.Parse(productQuantity.Text),
                Unit = productUnit.Text.Trim(),
                Photo = imagePath,
                CategoryId = (int)productCategory.SelectedValue,
                ManufacturerId = (int)productManufacturer.SelectedValue,
                SupplierId = (int)productSupplier.SelectedValue
            };
            var context = new ApplicationDbContext();
            if (selectedProductId == -1)
            {
                context.Products.Add(product);
            }
            else
            {
                product.Id = selectedProductId;
                context.Products.Update(product);
            }
            context.Products.Add(product);
            context.SaveChanges();

            MessageBox.Show("Информация о товаре сохранена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new ProductListPage());
        }

        private void changeImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png; *.jpg; *.jpeg; *.gif)|*.png; *.jpg; *.jpeg; *.gif)",
                Title = "Выберите изображение",
                CheckFileExists = true,
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                productImage.Source = bitmap;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var context = new ApplicationDbContext();

            Product product = context.Products.Where(p => p.Id == selectedProductId).FirstOrDefault();
            context.Products.Remove(product);
            context.SaveChanges();

            MessageBox.Show("Товар удалён успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new ProductListPage());
        }
    }
}
