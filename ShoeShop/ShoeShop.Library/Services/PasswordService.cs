using System.Security.Cryptography;
using System.Text;

namespace ShoeShop.Library.Services;

public static class PasswordService
{
    public static bool ValidatePassword(string password, string passwordHash)
    {
        using var sha256 = SHA256.Create();
        var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
        
        return passwordHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}