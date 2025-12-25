using System;
using System.Collections.Generic;

namespace ShoeShop.Library.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public byte[]? Bytes { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
