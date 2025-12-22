using ShoeShop.Library.Dtos.Products;
using ShoeShop.Library.Models;

namespace ShoeShop.Library.Extensions.Products;

public static class ProductExtension
{
    public static ProductDto ToProductDto(this Product product)
    {
        return new()
        {
            Article = product.Article,
            Title = product.Title,
            Description = product.Description,
            ManufacturerId = product.ManufacturerId,
            CategoryId = product.CategoryId,
            Discount = product.Discount,
            MeasurementUnit = product.MeasurementUnit,
            Price = product.Price,
            Quantity = product.Quantity,
            SupplierId = product.SupplierId,
            ImageId = product.ImageId,
        };
    }
}