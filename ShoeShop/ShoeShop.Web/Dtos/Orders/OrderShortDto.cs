namespace ShoeShop.Web.Dtos.Orders;

public class OrderShortDto
{
    public int Code { get; set; }

    public string Articles { get; set; }
    
    public DateOnly Date { get; set; }
    
    public decimal Total { get; set; }
}