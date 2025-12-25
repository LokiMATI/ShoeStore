namespace ShoeShop.Library.Dtos.Products;

/// <summary>
/// DTO товара
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Артикль
    /// </summary>
    public string Article { get; set; } = null!;

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Единица измерения
    /// </summary>
    public string MeasurementUnit { get; set; } = null!;

    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public short SupplierId { get; set; }

    /// <summary>
    /// Идентификатор производителя
    /// </summary>
    public int ManufacturerId { get; set; }

    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public short CategoryId { get; set; }

    /// <summary>
    /// Скидка
    /// </summary>
    public byte Discount { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public short Quantity { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Идентификатор изображения
    /// </summary>
    public int? ImageId { get; set; }
}