using ShoeShop.Library.Models;

namespace ShoeShop.Desktop.Dtos;

public class ProductDesktopDto
{
    public bool IsDiscounted { get; set; }
    
    public bool IsHas { get; set; }
    
    public decimal FinalPrice { get; set; }
    
    public Product Product { get; set; }
    
    public string ImageUrl { get; set; } = "../../images/picture.png";
    
    public bool IsDiscountedMoreThenFifteen { get; set; }
}