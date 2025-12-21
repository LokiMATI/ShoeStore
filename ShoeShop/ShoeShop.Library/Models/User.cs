namespace ShoeShop.Library.Models;

/// <summary>
/// Пользователь
/// </summary>
public partial class User
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Идентификатор роли
    /// </summary>
    public short RoleId { get; set; }

    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Хэш пароля
    /// </summary>
    public string Passwordhash { get; set; } = null!;

    /// <summary>
    /// Заказы
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    /// <summary>
    /// Роль
    /// </summary>
    public virtual Role Role { get; set; } = null!;
}
