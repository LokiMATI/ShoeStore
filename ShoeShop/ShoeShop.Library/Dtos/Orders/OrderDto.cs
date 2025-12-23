namespace ShoeShop.Library.Dtos.Orders;

/// <summary>
/// DTO для заказа
/// </summary>
public class OrderDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Дата заказа
    /// </summary>
    public DateOnly OrderDate { get; set; }

    /// <summary>
    /// Дата доставки
    /// </summary>
    public DateOnly DeliveryDate { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Код заказа
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Статус заказа
    /// </summary>
    public string Status { get; set; }
}