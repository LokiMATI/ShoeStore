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
using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Models;

namespace ShoeShop.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ShoeDbContext _context = new();
    public MainWindow()
    {
        InitializeComponent();

        LoadSortCategoryAsync();
        LoadManufacturersAsync();
        LoadProductsAsync();
    }

    private async Task LoadSortCategoryAsync()
    {
        string[] catrgories = ["По названию", "По поставщику", "По цене", "По убыванию цены"];
        
        SortComboBox.ItemsSource = catrgories;
    }
    
    private async Task LoadManufacturersAsync()
    {
        var manufacturers = await _context.Manufacturers.ToListAsync();
        
        manufacturers.Add(new()
        {
            ManufacturerId = 0,
            Title = "Все"
        });
        
        ManufacturerComboBox.ItemsSource = manufacturers;
        ManufacturerComboBox.DisplayMemberPath = "Title";
        
        ManufacturerComboBox.SelectedIndex = ManufacturerComboBox.Items.Count - 1;
    }
    
    private async Task LoadProductsAsync()
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Manufacturer)
            .Include(p => p.Supplier)
            .Include(p => p.Image)
            .AsQueryable();
        
        if (SortComboBox.SelectionBoxItem is not null)
            query = SortComboBox.SelectionBoxItem switch
            {
                "По названию" => query.OrderBy(p => p.Title),
                "По поставщику" => query.OrderBy(p => p.Manufacturer.Title),
                "По цене" => query.OrderByDescending(p => p.Price * (1 - (decimal)p.Discount / 100)),
                "По убыванию цены" => query.OrderBy(p => p.Price * (1 - (decimal)p.Discount / 100)),
                _ => query
            };
        
        if (ManufacturerComboBox.SelectedIndex != ManufacturerComboBox.Items.Count - 1 && ManufacturerComboBox.SelectedValue is Manufacturer manufacturer)
            query = query.Where(p => p.Manufacturer.ManufacturerId == manufacturer.ManufacturerId);

        if (int.TryParse(MaxPriceTextBox.Text, out int maxPrice))
            query = query.Where(p => p.Price <= maxPrice);

        query = DiscountCheckBox.IsChecked.Value ? query.Where(p => p.Discount > 0) : query;
        query = HasCheckBox.IsChecked.Value ? query.Where(p => p.Quantity > 0) : query;

        var products = await query.ToListAsync();
        foreach (var product in products.Where(p => p.Image is null))
            product.Image = new()
            {
                Name = @"images\picture.png",
            };

        ProductsListBox.ItemsSource = products;
    }

    private async Task UpdateProductsAsync()
    {
        await LoadProductsAsync();
    }

    private async void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        await UpdateProductsAsync();
    }

    private async void ComboBox_OnSelected(object sender, RoutedEventArgs e)
    {
        await UpdateProductsAsync();
    }

    private async void CheckBox_StateChanged(object sender, RoutedEventArgs e)
    {
        await UpdateProductsAsync();
    }
}