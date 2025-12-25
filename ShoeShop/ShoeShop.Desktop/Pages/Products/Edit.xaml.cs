using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ShoeShop.Desktop.Dtos;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Products;
using ShoeShop.Library.Models;
using ShoeShop.Library.Services;

namespace ShoeShop.Desktop.Pages.Products;

public partial class Edit : Page
{
    private Product _product = new();
    private string? _imagePath = null;
    public BitmapImage? Image = null;
    private bool _isCreated = true;
    private bool IsAllRequriedFieldsFilled =>
        string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
        string.IsNullOrWhiteSpace(MeasurementUnitTextBox.Text) ||
        string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
        SupplierComboBox.SelectionBoxItem is null ||
        ManufacturerComboBox.SelectionBoxItem is null ||
        CategoryComboBoxBox.SelectionBoxItem is null ||
        string.IsNullOrWhiteSpace(DiscountTextBox.Text) ||
        string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
        string.IsNullOrWhiteSpace(DescriptionTextBox.Text);
    
    public Edit()
    {
        App.MainWindow.Title = "Страница редактирования";
        App.PageTitle = "Страница редактирования";
        DataContext = this;
        InitializeComponent();
        LoadComboBoxes();

        ActionButton.Content = "Создать";
    }

    public Edit(string article)
    {
        if (_product.Image is not null)
        {
            using MemoryStream stream = new(_product.Image.Bytes);
            BitmapImage image = new();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            
            ProductImage.Source = image;
        }
        
        ShoeDbContext context = new();
        _product = context.Products
            .Include(p => p.Image)
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Include(p => p.Manufacturer)
            .FirstOrDefault(p => p.Article == article);
        if (_product is null)
            NavigationService.GoBack();
        
        App.MainWindow.Title = "Страница редактирования";
        App.PageTitle = "Страница редактирования";
        DataContext = this;
        InitializeComponent();
        LoadComboBoxes();
        
        _isCreated = false;
        
        FillFields();

        ActionButton.Content = "Редактировать";
    }

    private void ImageButton_OnClick(object sender, RoutedEventArgs e)
    {
        FileDialog dialog = new OpenFileDialog();

        if (dialog.ShowDialog() == true)
        {
            _imagePath = dialog.FileName;
            LoadImage();
            ProductImage.Visibility = Visibility.Visible;
        }
    }

    private async void ActionButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_isCreated)
                await CreateProductAsync();
            else
                await UpdateProductAsync();

            NavigationService.Navigate(new ProductsList());
        }
        catch (ArgumentException exception)
        {
            MessageBox.Show(exception.Message, "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (FileNotFoundException exception)
        {
            MessageBox.Show($"Файл не был найден. ({exception.Message})", "Файл не найден", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Возникла ошибка: {exception.Message}", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new ProductsList());
    }

    private void LoadImage()
    {
        try
        {
            FileInfo info = new(new Uri(_imagePath, UriKind.Relative).ToString());
            if (!info.Exists)
                throw new FileNotFoundException(info.FullName);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(_imagePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            ProductImage.Source = bitmap;
        }
        catch (FileNotFoundException exception)
        {
            MessageBox.Show($"Файл не был найден. ({exception.Message})", "Файл не найден", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Возникла ошибка: {exception.Message}", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private async Task CreateProductAsync()
    {
        ShoeDbContext context = new();
        if (IsAllRequriedFieldsFilled)
        {
            throw new ArgumentException("Не заполнены все важные поля.");
        }
        
        ProductCreateDto input = new();
        input.Title = TitleTextBox.Text.Trim();
        input.MeasurementUnit = MeasurementUnitTextBox.Text.Trim();
        if (decimal.TryParse(PriceTextBox.Text, out decimal price))
            input.Price = price;
        if (byte.TryParse(DiscountTextBox.Text, out byte discount))
            input.Discount = discount;
        if (short.TryParse(PriceTextBox.Text, out short quantity))
            input.Quantity = quantity;
        input.SupplierId = ((Supplier)SupplierComboBox.SelectionBoxItem).SupplierId;
        input.ManufacturerId = ((Manufacturer)ManufacturerComboBox.SelectionBoxItem).ManufacturerId;
        input.CategoryId = ((Category)CategoryComboBoxBox.SelectionBoxItem).CategoryId;
        input.Description = DescriptionTextBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(_imagePath))
        {
            FileInfo info = new(_imagePath);
            if (!info.Exists)
                throw new FileNotFoundException(info.FullName);

            ImageService imageService = new(context);
            var image = await imageService.AddAsync(await File.ReadAllBytesAsync(info.FullName));
            
            input.ImageId = image.ImageId;
        }

        ProductService productService = new(context);

        await productService.CreateAsync(input);
    }
    
    private async Task UpdateProductAsync()
    {
        ShoeDbContext context = new();
        if (IsAllRequriedFieldsFilled)
        {
            throw new ArgumentException("Не заполнены все важные поля.");
        }
        
        ProductUpdateDto input = new();
        input.Title = TitleTextBox.Text.Trim();
        input.MeasurementUnit = MeasurementUnitTextBox.Text.Trim();
        if (decimal.TryParse(PriceTextBox.Text, out decimal price))
            input.Price = price;
        if (byte.TryParse(DiscountTextBox.Text, out byte discount))
            input.Discount = discount;
        if (short.TryParse(PriceTextBox.Text, out short quantity))
            input.Quantity = quantity;
        input.SupplierId = ((Supplier)SupplierComboBox.SelectionBoxItem).SupplierId;
        input.ManufacturerId = ((Manufacturer)ManufacturerComboBox.SelectionBoxItem).ManufacturerId;
        input.Category = ((Category)CategoryComboBoxBox.SelectionBoxItem).Title;
        input.Description = DescriptionTextBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(_imagePath))
        {
            FileInfo info = new(_imagePath);
            if (!info.Exists)
                throw new FileNotFoundException(info.FullName);

            var bytes = await File.ReadAllBytesAsync(_imagePath);
            if (_product.Image is null || bytes != _product.Image.Bytes)
            {
                if (_product.Image is not null)
                    _product.Image.Bytes = bytes;
                else
                {
                    ImageService imageService = new(context);
                    var image = await imageService.AddAsync(await File.ReadAllBytesAsync(info.FullName));
            
                    input.ImageId = image.ImageId;
                }
            }
        }

        ProductService service = new(context);

        await service.UpdateAsync(_product.Article, input);
    }
    
    private void LoadComboBoxes()
    {
        LoadManufacturers();
        LoadSuppliers();
        LoadCategories();
    }
    
    private void LoadManufacturers()
    {
        ShoeDbContext context = new();
        var manufacturers = context.Manufacturers.ToList();
        ManufacturerComboBox.ItemsSource = manufacturers;
        ManufacturerComboBox.DisplayMemberPath = "Title";
        
        if (!_isCreated)
            ManufacturerComboBox.SelectedIndex = ManufacturerComboBox.Items.IndexOf(_product.Manufacturer);
    }
    
    private void LoadSuppliers()
    {
        ShoeDbContext context = new();
        var suppliers = context.Suppliers.ToList();
        SupplierComboBox.ItemsSource = suppliers;
        SupplierComboBox.DisplayMemberPath = "Title";
        
        if (!_isCreated)
            SupplierComboBox.SelectedIndex = SupplierComboBox.Items.IndexOf(_product.Supplier);
    }
    
    private void LoadCategories()
    {
        ShoeDbContext context = new();
        var categories = context.Categories.ToList();
        CategoryComboBoxBox.ItemsSource = categories;
        CategoryComboBoxBox.DisplayMemberPath = "Title";
        
        if (!_isCreated)
            CategoryComboBoxBox.SelectedIndex = CategoryComboBoxBox.Items.IndexOf(_product.Category);
    }
    
    private void FillFields()
    {
        TitleTextBox.Text = _product.Title;
        DescriptionTextBox.Text = _product.Description;
        MeasurementUnitTextBox.Text = _product.MeasurementUnit;
        PriceTextBox.Text = _product.Price.ToString();
        SupplierComboBox.SelectedIndex = SupplierComboBox.Items
            .IndexOf(SupplierComboBox.Items.Cast<Supplier>().FirstOrDefault(s 
                => s.SupplierId == _product.SupplierId));
        ManufacturerComboBox.SelectedIndex = ManufacturerComboBox.Items
            .IndexOf(ManufacturerComboBox.Items.Cast<Manufacturer>().FirstOrDefault(m 
                => m.ManufacturerId == _product.ManufacturerId));
        CategoryComboBoxBox.SelectedIndex = CategoryComboBoxBox.Items
            .IndexOf(CategoryComboBoxBox.Items.Cast<Category>().FirstOrDefault(c 
                => c.CategoryId == _product.CategoryId));
        DiscountTextBox.Text = _product.Discount.ToString();
        QuantityTextBox.Text = _product.Quantity.ToString();
    }
}