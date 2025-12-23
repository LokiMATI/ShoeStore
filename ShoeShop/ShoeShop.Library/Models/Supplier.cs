using System;
using System.Collections.Generic;

namespace ShoeShop.Library.Models;

public partial class Supplier
{
    public short SupplierId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
