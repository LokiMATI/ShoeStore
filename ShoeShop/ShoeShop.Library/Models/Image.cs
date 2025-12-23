using System;
using System.Collections.Generic;

namespace ShoeShop.Library.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
