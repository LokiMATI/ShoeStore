namespace ShoeShop.Library.Dtos.Orders;

public class OrderUpdateDto
{
    public DateOnly? DeliveryDate { get; set; } = null;

    public string? Status { get; set; } = null;
}