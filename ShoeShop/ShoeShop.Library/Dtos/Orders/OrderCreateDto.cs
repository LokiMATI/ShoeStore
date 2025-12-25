namespace ShoeShop.Library.Dtos.Orders;

public class OrderCreateDto
{
    public DateOnly? OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int UserId { get; set; }
    
    public List<string> Articles { get; set; } = new();
}