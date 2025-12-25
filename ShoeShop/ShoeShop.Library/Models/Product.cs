using System;
using System.Collections.Generic;

namespace ShoeShop.Library.Models;

public partial class Product
{
    public string Article { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string MeasurementUnit { get; set; } = null!;

    public decimal Price { get; set; }

    public short SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public short CategoryId { get; set; }

    public byte Discount { get; set; }

    public short Quantity { get; set; }

    public string Description { get; set; } = null!;

    public int? ImageId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Image? Image { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Supplier Supplier { get; set; } = null!;
}
