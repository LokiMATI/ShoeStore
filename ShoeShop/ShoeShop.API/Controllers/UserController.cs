using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoeShop.API.Properties;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Users;

namespace ShoeShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ShoeDbContext context) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserAuthDto input)
    {
        var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == input.Email);
        if (user is null)
            return Unauthorized();

        using var sha256 = SHA256.Create();
        var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(input.Password)));
        
        if (user.Passwordhash.Equals(hash))
            return Unauthorized(hash);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimsIdentity.DefaultRoleClaimType, user.Role.Title)
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