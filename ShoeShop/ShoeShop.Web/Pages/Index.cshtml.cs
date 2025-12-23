using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Models;
using ShoeShop.Library.Services;

namespace ShoeShop.Web.Pages.Products
{
    public class IndexModel(ShoeDbContext context, OrderService service) : PageModel
    {
        public IList<Product> Product { get;set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Manufacturer { get; set; } = "0";
        
        [BindProperty(SupportsGet = true)]
        public string? SortColumn { get; set; }

        [BindProperty(SupportsGet = true)] 
        public bool IsHasDiscountFlag { get; set; } = false;
        
        [BindProperty(SupportsGet = true)]
        public bool IsHasFlag { get; set; } = false;

        public async Task OnGetAsync()
        {
            var manufacturers = await context.Manufacturers.ToListAsync();
            manufacturers.Add(new Manufacturer{Title = "Все"});
            ViewData["Manufacturer"] = new SelectList(manufacturers, "ManufacturerId", "Title");
            
            var products = context.Products
                .Include(p => p.Category)
                .Include(p => p.Image)
                .Include(p => p.Manufacturer)
                .Include(p => p.Supplier).AsQueryable();
            
            products = IsHasFlag ? products.Where(p => p.Quantity > 0) : products;
            products = IsHasDiscountFlag ? products.Where(p => p.Discount > 0) : products;
            
            products = SortColumn switch
            {
                "price" => products.OrderBy(p => p.Price * (1 - (decimal)p.Discount / 100)),
                "price_desc" => products.OrderByDescending(p => p.Price * (1 - (decimal)p.Discount / 100)),
                "title" => products.OrderBy(p => p.Title),
                "supplier" => products.OrderBy(p => p.Supplier.Title),
                _ => products
            };
            
            if (!string.IsNullOrEmpty(SearchString))
                products = products.Where(p => p.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(Manufacturer) && int.TryParse(Manufacturer, out int manufacturerId) && manufacturerId != 0)
                products = products.Where(p => p.ManufacturerId == manufacturerId);
            
            Product = await products.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string article)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Гость" || role is null)
                return RedirectToPage("/Login");

            var order = await service.CreateAsync(new()
            {
                UserId = int.Parse(HttpContext.Session.GetString("UserId")),
                Articles = [article]
            });

            return RedirectToPage("/Index");
        }
    }
}
