using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Products;
using ShoeShop.Library.Extensions.Products;
using ShoeShop.Library.Models;
using ShoeShop.Library.Services;

namespace ShoeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ShoeDbContext context, ProductService service) : ControllerBase
    {
        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return await context.Products
                .Select(p => p.ToProductDto())
                .ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{article}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string article)
        {
            var product = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Supplier)
                .Include(p => p.Image)
                .FirstOrDefaultAsync(p => p.Article == article);

            if (product is null)
                return NotFound();

            return product.ToProductDto();
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{article}")]
        public async Task<ActionResult<ProductDto>> PutProduct(string article, ProductUpdateDto input)
        {
            try
            {
                var product = await service.UpdateAsync(article, input);

                if (product is null)
                    return NotFound();

                return product.ToProductDto();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductCreateDto input)
        {
            try
            {
                var product = await service.CreateAsync(input);
                return CreatedAtAction(nameof(GetProduct), new { article = product.Article }, product.ToProductDto());
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{article}")]
        public async Task<IActionResult> DeleteProduct(string article)
        {
            var product = await context.Products.FindAsync(article);
            if (product is null)
                return NotFound();

            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
