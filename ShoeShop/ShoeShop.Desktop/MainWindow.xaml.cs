using System.Windows;
using ShoeShop.Desktop.Pages;

namespace ShoeShop.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainFrame.Navigate(new AuthPage());
    }
}