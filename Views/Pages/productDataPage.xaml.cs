using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp1.Models;

namespace WpfApp1.Views.Pages
{
    public partial class productDataPage : Page
    {
        private string filePath;
        private int selectedProductId = -1;

        public productDataPage()
        {
            InitializeComponent();
            LoadData();
            buttonDelete.Visibility = Visibility.Collapsed;
        }

        public productDataPage(int productId) : this()
        {
            selectedProductId = productId;
            LoadProduct(selectedProductId);
            buttonDelete.Visibility = Visibility.Visible;
        }

        private void LoadProduct(int productId)
        {
            using (var context = new ApplicationDbContext())
            {
                Product product = context.Products.Find(productId);
                if (product == null) return;

                productName.Text = product.Name;
                productDescription.Text = product.Description;
                productArticle.Text = product.Article;
                productPrice.Value = (double)product.Price;
                productDiscount.Value = (double)product.Discount;
                productQuantity.Value = product.StockQuantity;
                productUnit.Text = product.Unit;
                productCategory.SelectedValue = product.CategoryId;
                productManufacturer.SelectedValue = product.ManufacturerId;
                productSupplier.SelectedValue = product.SupplierId;

                string photoPath = string.IsNullOrEmpty(product.Photo)
                    ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shopResources", "picture.png")
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shopResources", product.Photo);

                if (File.Exists(photoPath))
                {
                    productImage.Source = new BitmapImage(new Uri(photoPath));
                    filePath = photoPath;
                }
            }
        }

        private string SaveImage(string sourcePath)
        {
            var fileInfo = new FileInfo(sourcePath);
            var destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shopResources", fileInfo.Name);
            File.Copy(sourcePath, destPath, overwrite: true);
            return fileInfo.Name;
        }

        private void LoadData()
        {
            using (var context = new ApplicationDbContext())
            {
                productCategory.ItemsSource = context.Categories.ToList();
                productManufacturer.ItemsSource = context.Manufacturers.ToList();
                productSupplier.ItemsSource = context.Suppliers.ToList();
            }

            productCategory.SelectedIndex = 0;
            productManufacturer.SelectedIndex = 0;
            productSupplier.SelectedIndex = 0;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(productName.Text) ||
                string.IsNullOrWhiteSpace(productArticle.Text) ||
                string.IsNullOrWhiteSpace(productDescription.Text) ||
                productPrice.Value == null ||
                productDiscount.Value == null ||
                productQuantity.Value == null ||
                string.IsNullOrWhiteSpace(productUnit.Text))
            {
                MessageBox.Show("Заполните все поля для продолжения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string savedPhotoName = null;
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                savedPhotoName = SaveImage(filePath);
            }

            using (var context = new ApplicationDbContext())
            {
                Product product;

                if (selectedProductId == -1)
                {
                    product = new Product();
                    context.Products.Add(product);
                }
                else
                {
                    product = context.Products.Find(selectedProductId);
                    if (product == null)
                    {
                        MessageBox.Show("Товар не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                product.Name = productName.Text.Trim();
                product.Article = productArticle.Text.Trim();
                product.Description = productDescription.Text.Trim();
                product.Price = (decimal)productPrice.Value;
                product.Discount = (decimal)productDiscount.Value;
                product.StockQuantity = (int)productQuantity.Value;
                product.Unit = productUnit.Text.Trim();
                if (savedPhotoName != null)
                    product.Photo = savedPhotoName;

                product.CategoryId = (int)productCategory.SelectedValue;
                product.ManufacturerId = (int)productManufacturer.SelectedValue;
                product.SupplierId = (int)productSupplier.SelectedValue;

                context.SaveChanges();
            }

            MessageBox.Show("Информация о товаре сохранена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new ProductListPage());
        }

        private void changeImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png; *.jpg; *.jpeg; *.gif)|*.png; *.jpg; *.jpeg; *.gif",
                Title = "Выберите изображение",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                productImage.Source = new BitmapImage(new Uri(filePath));
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProductId == -1) return;

            using (var context = new ApplicationDbContext())
            {
                var product = context.Products.Find(selectedProductId);
                if (product != null)
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Товар удалён успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new ProductListPage());
        }
    }
}