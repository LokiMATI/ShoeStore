namespace ShoeShop.Library.Models;

/// <summary>
/// Производитель
/// </summary>
public partial class Manufacturer
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int ManufacturerId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Производимые продукты
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
