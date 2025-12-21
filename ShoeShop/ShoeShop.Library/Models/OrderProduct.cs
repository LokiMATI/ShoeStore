namespace ShoeShop.Library.Models;

/// <summary>
/// Продукт заказа
/// </summary>
public partial class OrderProduct
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Артикль
    /// </summary>
    public string Article { get; set; } = null!;
    
    /// <summary>
    /// Количество продуктов
    /// </summary>
    public byte Quantity { get; set; }

    /// <summary>
    /// Продукт
    /// </summary>
    public virtual Product ArticleNavigation { get; set; } = null!;

    /// <summary>
    /// Заказ
    /// </summary>
    public virtual Order Order { get; set; } = null!;
}
