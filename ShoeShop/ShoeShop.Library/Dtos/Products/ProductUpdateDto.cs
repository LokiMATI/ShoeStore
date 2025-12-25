namespace ShoeShop.Library.Dtos.Products;

/// <summary>
/// DTO для обновления товара
/// </summary>
public class ProductUpdateDto
{
    /// <summary>
    /// Название
    /// </summary>
    public string? Title { get; set; }  = null;

    /// <summary>
    /// Единица измерения
    /// </summary>
    public string? MeasurementUnit { get; set; }  = null;

    /// <summary>
    /// Цена
    /// </summary>
    public decimal? Price { get; set; }  = null;

    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public short? SupplierId { get; set; }  = null;

    /// <summary>
    /// Идентификатор производителя
    /// </summary>
    public int? ManufacturerId { get; set; }  = null;

    /// <summary>
    /// Категория
    /// </summary>
    public string? Category { get; set; }  = null;

    /// <summary>
    /// Скидка
    /// </summary>
    public byte? Discount { get; set; }  = null;

    /// <summary>
    /// Количество
    /// </summary>
    public short? Quantity { get; set; }  = null;

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }  = null;

    /// <summary>
    /// Идентификатор изображения
    /// </summary>
    public int? ImageId { get; set; }  = null;
}