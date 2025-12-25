using System;
using System.Collections.Generic;

namespace ShoeShop.Library.Models;

public partial class OrderProduct
{
    public int OrderId { get; set; }

    public string Article { get; set; } = null!;

    public byte Quantity { get; set; }

    public virtual Product ArticleNavigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
