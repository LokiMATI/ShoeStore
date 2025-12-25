using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Products;
using ShoeShop.Library.Models;

namespace ShoeShop.Library.Services;

/// <summary>
/// Сервис для товаров
/// </summary>
public class ProductService(ShoeDbContext context)
{
    /// <summary>
    /// Создание товара
    /// </summary>
    /// <param name="input">Входные данные</param>
    /// <returns>Созданный товар</returns>
    public async Task<Product?> CreateAsync(ProductCreateDto input)
    {
        string article;
        do
        {
            article = GenerateArticle();
        } while (context.Products.Any(p => p.Article == article));

        Product product = new()
        {
            Article = article,
            Title = input.Title,
            Description = input.Description,
            Price = input.Price,
            MeasurementUnit = input.MeasurementUnit,
            SupplierId = input.SupplierId,
            CategoryId = input.CategoryId,
            ManufacturerId = input.ManufacturerId,
            ImageId = input.ImageId,
            Quantity = input.Quantity,
            Discount = input.Discount
        };
        
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        
        return product;
    }

    /// <summary>
    /// Обновление товара
    /// </summary>
    /// <param name="article">Артикль</param>
    /// <param name="input">Входные данные</param>
    /// <returns>Обновлённый товар</returns>
    public async Task<Product?> UpdateAsync(string article,ProductUpdateDto input)
    {
        var product = context.Products
            .Include(p => p.Category)
            .Include(p => p.Image)
            .Include(p => p.Manufacturer)
            .Include(p => p.Supplier)
            .FirstOrDefault(p => p.Title == input.Title);
        
        if (product is null)
            return null;
        
        if (input.Title is not null)
            product.Title = input.Title;
        
        if (input.Description is not null)
            product.Description = input.Description;
        
        if (input.MeasurementUnit is not null)
            product.MeasurementUnit = input.MeasurementUnit;
        
        if (input.Price is not null)
            product.Price = input.Price.Value;
        
        if (input.SupplierId is not null)
            product.SupplierId = input.SupplierId.Value;
        
        if (input.ManufacturerId is not null)
            product.ManufacturerId = input.ManufacturerId.Value;

        if (input.Category is not null)
        {
            // Получение категории по названию
            var category = await context.Categories
                .FirstOrDefaultAsync(c => c.Title == input.Category);

            if (category is not null)
                product.CategoryId = category.CategoryId;
        }

        if (input.Discount is not null)
            product.Discount = input.Discount.Value;
        
        if (input.Quantity is not null)
            product.Quantity = input.Quantity.Value;
        
        if (input.ImageId is not null)
            product.ImageId = input.ImageId;
        
        context.Update(product);
        await context.SaveChangesAsync();
        
        return product;
    }
    
    private static string GenerateArticle()
    {
        return $"{(char)Random.Shared.Next('A', 'Z' + 1)}{Random.Shared.Next(0, 10)}{Random.Shared.Next(0, 10)}{Random.Shared.Next(0, 10)}{(char)Random.Shared.Next('A', 'Z' + 1)}{Random.Shared.Next(0, 10)}";
    }
}