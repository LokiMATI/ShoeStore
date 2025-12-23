using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ShoeShop.API.Properties;

/// <summary>
/// Настройки JWT-токена (временно)
/// </summary>
public class AuthOptions
{
    public const string ISSUER = "ShoeShopWebApi";
    public const string AUDIENCE = "OfficalViewShoeShop";
    private const string KEY = "ShoeShopSecretKey_wdadnoiawndncwncw";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
        => new(Encoding.UTF8.GetBytes(KEY));
}