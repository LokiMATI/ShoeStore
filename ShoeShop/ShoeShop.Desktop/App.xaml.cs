using System.Configuration;
using System.Data;
using System.Windows;
using ShoeShop.Library.Contexts;

namespace ShoeShop.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static string? Role { get; set; } = null;

    public static bool IsCanEdit = false;

    public static Window MainWindow { get; set; }
    
    public static string PageTitle { get; set; } = string.Empty;
}