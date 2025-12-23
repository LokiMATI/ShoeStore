using ShoeShop.Library.Dtos.Orders;
using ShoeShop.Library.Models;

namespace ShoeShop.Library.Extensions.Orders;

public static class OrderExtension
{
    public static OrderDto ToOrderDto(this Order order)
    {
        return new()
        {
            Id = order.OrderId,
            OrderDate = order.OrderDate,
            DeliveryDate = order.DeliveryDate,
            UserId = order.UserId,
            Code = order.Code,
            Status = order.Status,
        };
    }
}