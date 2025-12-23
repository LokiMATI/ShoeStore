using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Models;
using ShoeShop.Web.Dtos.Orders;

namespace ShoeShop.Web.Pages.Orders
{
    public class IndexModel(ShoeDbContext context) : PageModel
    {
        public IList<OrderShortDto> Order { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Гость" || role is null)
                return RedirectToPage("~/Login");


            var orders = context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(o => o.ArticleNavigation)
                .AsQueryable();

            orders = role switch
            {
                "Клиент" => orders.Where(o => o.UserId == int.Parse(HttpContext.Session.GetString("UserId"))),
                _ => orders
            };
            
            Order = await orders.Select(o => new OrderShortDto()
            {
                Code = o.Code,
                Date = o.OrderDate,
                Articles = string.Join('\n', o.OrderProducts.Select(op => op.Article).ToList()),
                Total = o.OrderProducts.Sum(op => op.ArticleNavigation.Price * (1 - (decimal)op.ArticleNavigation.Discount / 100) * op.Quantity),
            }).ToListAsync();
            
            return Page();
        }
    }
}
