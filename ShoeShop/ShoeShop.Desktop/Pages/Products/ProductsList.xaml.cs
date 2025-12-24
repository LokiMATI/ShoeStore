using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Desktop.Dtos;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Models;

namespace ShoeShop.Desktop.Pages.Products;

public partial class ProductsList : Page
{
    private readonly ShoeDbContext _context = new();
    
    public ProductsList()
    {
        InitializeComponent();
        
        EditStackPanel.Visibility = App.IsCanEdit ? Visibility.Visible : Visibility.Collapsed;
        
        LoadSortCategory();
        LoadManufacturers();
        
        LoadProducts();
    }
    
    private void LoadSortCategory()
    {
        string[] catrgories = ["По названию", "По поставщику", "По цене", "По убыванию цены"];
        
        SortComboBox.ItemsSource = catrgories;
        SortComboBox.SelectedIndex = 0;
    }
    
    private void LoadManufacturers()
    {
        var manufacturers = _context.Manufacturers.ToList();
        
        manufacturers.Add(new()
        {
            ManufacturerId = 0,
            Title = "Все"
        });
        
        ManufacturerComboBox.ItemsSource = manufacturers;
        ManufacturerComboBox.DisplayMemberPath = "Title";
        ManufacturerComboBox.SelectedIndex = ManufacturerComboBox.Items.Count - 1;
    }
    
    private void LoadProducts()
    {
        var query = _context.Products
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

        var products = query.Select(p => new ProductDesktopDto()
        {
            IsDiscounted = p.Discount > 0,
            IsHas = p.Quantity > 0,
            FinalPrice = p.Price * (1 - (decimal)p.Discount / 100),
            Product = p
        }).ToList();
        foreach (var product in products)
        {
            if (product.Product.Image is not null)
                product.ImageUrl = $@"..\..\{product.Product.Image.Name}";
        }
            

        ProductsListBox.ItemsSource = products;
    }

    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        LoadProducts();
    }

    private void ComboBox_OnSelected(object sender, RoutedEventArgs e)
    {
        LoadProducts();
    }

    private void CheckBox_StateChanged(object sender, RoutedEventArgs e)
    {
        LoadProducts();
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

        NavigationService.Navigate(new Edit(product));
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ProductsListBox.SelectedItems.Count == 0)
        {
            MessageBox.Show("Для удаление продуктов выберите хотя бы один продукт.", "Не выбран элемент", MessageBoxButton.OK, MessageBoxImage.Stop);
            return;
        }
        
        _context.Products.RemoveRange(ProductsListBox.SelectedItems as IEnumerable<Product>);
        LoadProducts();
    }
}