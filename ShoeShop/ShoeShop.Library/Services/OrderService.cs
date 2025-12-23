using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Orders;
using ShoeShop.Library.Models;

namespace ShoeShop.Library.Services;

public class OrderService(ShoeDbContext context)
{
    public async Task<Order?> UpdateAsync(int id, OrderUpdateDto input)
    {
        var order = await context.Orders
            .Include(o => o.OrderProducts)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.OrderId == id);
        
        if (order is null)
            return null;
        
        if (input.Status is not null)
            order.Status = input.Status;
        
        if (input.DeliveryDate is not null)
            order.DeliveryDate = input.DeliveryDate.Value;
        
        context.Update(order);
        await context.SaveChangesAsync();
        
        return order;
    } 
}