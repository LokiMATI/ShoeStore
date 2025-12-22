namespace ShoeShop.Library.Models;

/// <summary>
/// Товар
/// </summary>
public partial class Product
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
    /// Единицы измерения
    /// </summary>
    public string MeasurementUnit { get; set; } = null!;

    /// <summary>
    /// Цена
    /// </summary>
    public short Price { get; set; }

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
    /// Скида
    /// </summary>
    public byte Discount { get; set; } = 0;

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

    /// <summary>
    /// Категория
    /// </summary>
    public virtual Category Category { get; set; } = null!;

    /// <summary>
    /// Изображение
    /// </summary>
    public virtual Image? Image { get; set; }

    /// <summary>
    /// Производитель
    /// </summary>
    public virtual Manufacturer Manufacturer { get; set; } = null!;

    /// <summary>
    /// Заказы продукта
    /// </summary>
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    /// <summary>
    /// Поставщик
    /// </summary>
    public virtual Supplier Supplier { get; set; } = null!;
}
