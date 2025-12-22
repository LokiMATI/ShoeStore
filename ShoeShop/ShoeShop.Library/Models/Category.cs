namespace ShoeShop.Library.Models;

/// <summary>
/// Категория
/// </summary>
public partial class Category
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public short CategoryId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Продуты категории
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
