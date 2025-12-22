namespace ShoeShop.Library.Models;

/// <summary>
/// Изображение
/// </summary>
public partial class Image
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int ImageId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Продукты
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
