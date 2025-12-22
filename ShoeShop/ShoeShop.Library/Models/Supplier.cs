namespace ShoeShop.Library.Models;

/// <summary>
/// Поставщик
/// </summary>
public partial class Supplier
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public short SupplierId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Продукты
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
