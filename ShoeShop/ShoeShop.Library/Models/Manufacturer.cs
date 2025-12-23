using System;
using System.Collections.Generic;

namespace ShoeShop.Library.Models;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
