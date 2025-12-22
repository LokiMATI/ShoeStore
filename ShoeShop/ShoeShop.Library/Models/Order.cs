namespace ShoeShop.Library.Models;

/// <summary>
/// Заказ
/// </summary>
public partial class Order
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Дата оформления заказа
    /// </summary>
    public DateOnly OrderDate { get; set; }

    /// <summary>
    /// Дата доставки заказа
    /// </summary>
    public DateOnly DeliveryDate { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Код получения
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Статус заказа
    /// </summary>
    public string Status { get; set; } = "Новый";

    /// <summary>
    /// Продукты заказа
    /// </summary>
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    /// <summary>
    /// Клиент
    /// </summary>
    public virtual User User { get; set; } = null!;
}
