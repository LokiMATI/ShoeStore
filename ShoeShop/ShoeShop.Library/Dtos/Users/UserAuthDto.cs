namespace ShoeShop.Library.Dtos.Users;

/// <summary>
/// DTO для авторизации
/// </summary>
public class UserAuthDto
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}