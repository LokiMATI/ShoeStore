using System.Windows.Media.Imaging;
using ShoeShop.Library.Models;

namespace ShoeShop.Desktop.Dtos;

public class ProductDesktopDto
{
    public bool IsDiscounted { get; set; }
    
    public bool IsHas { get; set; }
    
    public decimal FinalPrice { get; set; }
    
    public Product Product { get; set; }
    
    public BitmapImage Image { get; set; }
    
    public bool IsDiscountedMoreThenFifteen { get; set; }
}