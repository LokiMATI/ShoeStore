using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Orders;
using ShoeShop.Library.Extensions.Orders;
using ShoeShop.Library.Models;
using ShoeShop.Library.Services;

namespace ShoeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(ShoeDbContext context, OrderService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            return await context.Orders.Select(o => o.ToOrderDto()).ToListAsync();
        }

        [Authorize]
        [HttpGet("{email}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrder(string email)
        {
            var orders = await context.Orders
                .Include(o => o.User)
                .Where(o => o.User.Email == email)
                .Select(o => o.ToOrderDto())
                .ToListAsync();

            return orders;
        }

        [Authorize(Roles = "Администратор, Менеджер")]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> PutOrder(int id, [FromBody]OrderUpdateDto input)
        {
            try
            {
                var order = await service.UpdateAsync(id, input);
                
                if (order is null)
                    return NotFound();

                return order.ToOrderDto();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
