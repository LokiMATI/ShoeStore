using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Users;
using ShoeShop.Library.Services;

namespace ShoeShop.Web.Pages;

public class Login(ShoeDbContext context) : PageModel
{
    public IActionResult OnGet()
    {
        ViewData["RoleId"] = new SelectList(context.Roles, "RoleId", "RoleId");
        return Page();
    }

    [BindProperty]
    public UserAuthDto User { get; set; } = default!;

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public IActionResult OnGetLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostLoginAsync()
    {
        var user = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == User.Email);

        if (user is null || !PasswordService.ValidatePassword(User.Password, user.Passwordhash))
            return Page();

        HttpContext.Session.SetString("Fullname", $"{user.Surname} {user.Name} {user.Patronymic ?? string.Empty}".Trim());
        HttpContext.Session.SetString("Role", user.Role.Title);
        HttpContext.Session.SetString("UserId", user.UserId.ToString());
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostGuestAsync()
    {
        HttpContext.Session.Clear();
        HttpContext.Session.SetString("Role", "Гость");
        return RedirectToPage("/Index");
    }
}