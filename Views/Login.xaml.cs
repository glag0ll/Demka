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
using WpfApp1.Models;

namespace WpfApp1.Views
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void guestButton_Click(object sender, RoutedEventArgs e)
        {
            var GuestWin = new guestWindow();
            GuestWin.Show();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginForm.Text.ToString();
            string password = passwordForm.Password.ToString();

            if (login == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Заполните все поля для продолжения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var context = new ApplicationDbContext();
            User user = context.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();
            if (user != null)
            {
                MessageBox.Show($"Добро пожаловать {user.FullName}!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Properties["user"] = user;

                var menuWin = new menuWindow();
                menuWin.Show();

                foreach(Window w in Application.Current.Windows)
                {
                    if (w is not menuWindow)
                    { 
                        w.Close();                    
                    }
                }

            }
            else
            {
                MessageBox.Show("Неправильно введены логин или пароль", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
