using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Desktop.Pages.Products;
using ShoeShop.Library.Contexts;

namespace ShoeShop.Desktop.Pages;

public partial class AuthPage : Page
{
    private readonly ShoeDbContext _context;
    
    public AuthPage()
    {
        App.MainWindow.Title = "Авторизация";
        _context = new();
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        LoginButton.IsEnabled = false;
        if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
        {
            MessageBox.Show("Поля для авторизации пустые!", "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Error);
            LoginButton.IsEnabled = true;
            return;
        }
        
        var user = _context.Users
            .Include(u => u.Role) 
            .FirstOrDefault(u => u.Email == LoginTextBox.Text.Trim());

        if (user is null)
        {
            MessageBox.Show("Такого пользователя нет!", "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Error);
            LoginButton.IsEnabled = true;
            return;
        }
        
        using var sha256 = SHA256.Create();
        var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(PasswordBox.Password))).Replace("-", "");

        if (!user.Passwordhash.Equals(hash, StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("Неверный пароль!", "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Error);
            PasswordBox.Password = string.Empty;
            LoginButton.IsEnabled = true;
            return;
        }

        MessageBox.Show("Вы зашли в аккаунт!", $"Добро пожаловать, {user.Name}!", MessageBoxButton.OK,
            MessageBoxImage.Information);

        App.Role = user.Role.Title;
        
        App.IsCanEdit = new List<string> {"Администратор", "Менеджер"}.Contains(user.Role.Title);
        
        NavigationService.Navigate(new ProductsList());
    }
}