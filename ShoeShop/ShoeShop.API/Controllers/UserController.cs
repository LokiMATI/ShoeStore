using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.API.Services;
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

        return JwtService.GenerateJwtToken(user.Email, user.Role.Title);
    }

    
}