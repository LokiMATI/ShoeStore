using System.Security.Cryptography;
using System.Text;

namespace ShoeShop.Library.Services;

/// <summary>
/// Сервис для работы с паролями
/// </summary>
public static class PasswordService
{
    /// <summary>
    /// Проверка пароля
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <param name="passwordHash">Хэш пароля</param>
    /// <returns>True, если пароль совпадает со значением в хэше</returns>
    public static bool VerificationPassword(string password, string passwordHash)
    {
        using var sha256 = SHA256.Create();
        var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
        
        return passwordHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}