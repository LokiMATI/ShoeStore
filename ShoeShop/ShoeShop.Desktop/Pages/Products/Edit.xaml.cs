using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
    private string _imagePath = "";
    private bool _isCreated = true;
    
    private readonly ShoeDbContext _context = new();
    
    public Edit(ProductDesktopDto? product = null)
    {
        DataContext = this;
        if (product is not null)
        {
            _product = product.Product;
            _isCreated = false;
            if (_product.Image is not null)
                _imagePath = $"{product.ImageUrl}";

            LoadImage();
        }
        
        InitializeComponent();
        
        LoadManufacturers();
        LoadSuppliers();
        LoadCategories();

        ActionButton.Content = _isCreated ? "Создать" : "Редактировать";
    }

    private void ImageButton_OnClick(object sender, RoutedEventArgs e)
    {
        FileDialog dialog = new OpenFileDialog();

        if (dialog.ShowDialog() == true)
        {
            _imagePath = dialog.FileName;
            LoadImage();
            Image.Visibility = Visibility.Visible;
        }
    }

    private void LoadImage()
    {
        try
        {
            FileInfo info = new(new Uri(_imagePath, UriKind.Relative).ToString());
            if (!info.Exists)
                throw new FileNotFoundException(info.FullName);

            Image.Source = new BitmapImage(new Uri(_imagePath, UriKind.Relative));
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

    private void ActionButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_isCreated)
                CreateProduct();
            else
                UpdateProduct();

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

    private void CreateProduct()
    {
        if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
            string.IsNullOrWhiteSpace(MeasurementUnitTextBox.Text) ||
            string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
            SupplierComboBox.SelectionBoxItem is null ||
            ManufacturerComboBox.SelectionBoxItem is null ||
            CategoryComboBoxBox.SelectionBoxItem is null ||
            string.IsNullOrWhiteSpace(DiscountTextBox.Text) ||
            string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
            string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
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

        if (!string.IsNullOrWhiteSpace(_imagePath) && _product.Image is not null && _product.Image.ImageId != 0)
        {
            FileInfo info = new(_imagePath);
            if (!info.Exists)
                throw new FileNotFoundException(info.FullName);

            Library.Models.Image image = new();
            image.ImageId = _context.Images.Count() + 1;
            image.Name = $"images/{image.ImageId}.{info.Extension}";
            image.Bytes = File.ReadAllBytes(_imagePath);
            
            File.Copy(info.FullName, $"../../{image.Name}", true);
            
            _context.Images.Add(image);
            _context.SaveChanges();
            
            input.ImageId = image.ImageId;
        }

        ProductService service = new(_context);

        service.CreateAsync(input);
    }
    
    private void UpdateProduct()
    {
        if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
            string.IsNullOrWhiteSpace(MeasurementUnitTextBox.Text) ||
            string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
            SupplierComboBox.SelectionBoxItem is null ||
            ManufacturerComboBox.SelectionBoxItem is null ||
            CategoryComboBoxBox.SelectionBoxItem is null ||
            string.IsNullOrWhiteSpace(DiscountTextBox.Text) ||
            string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
            string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
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

        if (!string.IsNullOrWhiteSpace(_imagePath) && _product.Image is not null && _product.Image.ImageId != 0)
        {
            FileInfo info = new(_imagePath);
            if (!info.Exists)
                throw new FileNotFoundException(info.FullName);

            Library.Models.Image image = new();
            image.ImageId = _context.Images.Count() + 1;
            image.Name = $"images/{image.ImageId}.{info.Extension}";
            image.Bytes = File.ReadAllBytes(_imagePath);
            
            _context.Images.Add(image);
            _context.SaveChanges();
            
            input.ImageId = image.ImageId;
        }

        ProductService service = new(_context);

        service.UpdateAsync(_product.Article, input);

    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new ProductsList());
    }
    
    private void LoadManufacturers()
    {
        var manufacturers = _context.Manufacturers.ToList();
        ManufacturerComboBox.ItemsSource = manufacturers;
        ManufacturerComboBox.DisplayMemberPath = "Title";
        
        if (!_isCreated)
            ManufacturerComboBox.SelectedIndex = ManufacturerComboBox.Items.IndexOf(_product.Manufacturer);
    }
    
    private void LoadSuppliers()
    {
        var suppliers = _context.Suppliers.ToList();
        SupplierComboBox.ItemsSource = suppliers;
        SupplierComboBox.DisplayMemberPath = "Title";
        
        if (!_isCreated)
            SupplierComboBox.SelectedIndex = SupplierComboBox.Items.IndexOf(_product.Supplier);
    }
    
    private void LoadCategories()
    {
        var categories = _context.Categories.ToList();
        CategoryComboBoxBox.ItemsSource = categories;
        CategoryComboBoxBox.DisplayMemberPath = "Title";
        
        if (!_isCreated)
            CategoryComboBoxBox.SelectedIndex = CategoryComboBoxBox.Items.IndexOf(_product.Category);
    }
}