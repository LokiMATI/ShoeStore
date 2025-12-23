using System;
using System.Collections.Generic;

namespace ShoeShop.Library.Models;

public partial class Role
{
    public short RoleId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
