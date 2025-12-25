using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ShoeShop.API.Properties;

namespace ShoeShop.API.Services;

/// <summary>
/// Сервис для работы с JWT-токенами
/// </summary>
public static class JwtService
{
    /// <summary>
    /// Генерация JWT-токена
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="role">Роль</param>
    /// <returns>JWT-токен</returns>
    public static string GenerateJwtToken(string email, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email),
            new(ClaimsIdentity.DefaultRoleClaimType, role)
        };
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(120)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}