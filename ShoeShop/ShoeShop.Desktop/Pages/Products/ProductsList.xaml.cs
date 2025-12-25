using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Desktop.Dtos;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Models;

namespace ShoeShop.Desktop.Pages.Products;

public partial class ProductsList : Page
{
    
    public ProductsList()
    {
        App.MainWindow.Title = "Товары";
        InitializeComponent();
        
        EditStackPanel.Visibility = App.IsCanEdit ? Visibility.Visible : Visibility.Collapsed;
        
        LoadSortCategory();
        LoadManufacturers();
        
        LoadProductsAsync();
    }
    
    private void LoadSortCategory()
    {
        string[] catrgories = ["По названию", "По поставщику", "По цене", "По убыванию цены"];
        
        SortComboBox.ItemsSource = catrgories;
        SortComboBox.SelectedIndex = 0;
    }
    
    private void LoadManufacturers()
    {
        ShoeDbContext context = new();
        var manufacturers = context.Manufacturers.ToList();
        
        manufacturers.Add(new()
        {
            ManufacturerId = 0,
            Title = "Все"
        });
        
        ManufacturerComboBox.ItemsSource = manufacturers;
        ManufacturerComboBox.DisplayMemberPath = "Title";
        ManufacturerComboBox.SelectedIndex = ManufacturerComboBox.Items.Count - 1;
    }
    
    private async Task LoadManufacturersAsync()
    {
        ShoeDbContext context = new();
        var manufacturers = await context.Manufacturers.ToListAsync();
        
        manufacturers.Add(new()
        {
            ManufacturerId = 0,
            Title = "Все"
        });
        
        ManufacturerComboBox.ItemsSource = manufacturers;
        ManufacturerComboBox.DisplayMemberPath = "Title";
    }
    
    private async Task LoadProductsAsync()
    {
        ShoeDbContext context = new();
        var query = context.Products
            .Include(p => p.Category)
            .Include(p => p.Manufacturer)
            .Include(p => p.Supplier)
            .Include(p => p.Image)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            query = query.Where(p => p.Title.Contains(SearchTextBox.Text));
        
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

        var products = await query.Select(p => new ProductDesktopDto()
        {
            IsDiscounted = p.Discount > 0,
            IsHas = p.Quantity > 0,
            FinalPrice = p.Price * (1 - (decimal)p.Discount / 100),
            Product = p,
            IsDiscountedMoreThenFifteen = p.Discount > 15,
        }).ToListAsync();
        foreach (var product in products)
        {
            if (product.Product.Image is not null)
                product.ImageUrl = $@"..\..\{product.Product.Image.Name}";
        }
            

        ProductsListBox.ItemsSource = products;
    }

    private async void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        await LoadProductsAsync();
    }

    private async void ComboBox_OnSelected(object sender, RoutedEventArgs e)
    {
        await LoadProductsAsync();
    }

    private async void CheckBox_StateChanged(object sender, RoutedEventArgs e)
    {
        await LoadProductsAsync();
    }

    private void CreateButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Edit());
    }

    private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ProductsListBox.SelectedItem is null)
        {
            MessageBox.Show("Для изменения продукта выберите сам продукт.", "Не выбран элемент", MessageBoxButton.OK, MessageBoxImage.Stop);
            return;
        }
        
        var product = ProductsListBox.SelectedItem as ProductDesktopDto;

        NavigationService.Navigate(new Edit(product.Product.Article));
    }

    private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        ShoeDbContext context = new();
        if (ProductsListBox.SelectedItems.Count == 0)
        {
            MessageBox.Show("Для удаление продуктов выберите хотя бы один продукт.", "Не выбран элемент", MessageBoxButton.OK, MessageBoxImage.Stop);
            return;
        }

        var products = ProductsListBox.SelectedItems.Cast<ProductDesktopDto>().ToList()
            .Select(p => p.Product);
        
        context.Products.RemoveRange(products);
        await context.SaveChangesAsync();
        LoadProductsAsync();
    }
}