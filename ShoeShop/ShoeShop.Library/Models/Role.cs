namespace ShoeShop.Library.Models;

/// <summary>
/// Роль
/// </summary>
public partial class Role
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public short RoleId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Пользователи
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
