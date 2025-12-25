using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Dtos.Orders;
using ShoeShop.Library.Models;

namespace ShoeShop.Library.Services;

public class OrderService(ShoeDbContext context)
{
    public async Task<Order?> CreateAsync(OrderCreateDto input)
    {
        int code;
        do
        {
            code = Random.Shared.Next(100, 1000);
        } while (context.Orders.Any(o => o.Code == code));

        Order order = new()
        {
            OrderId = context.Orders.Max(o => o.OrderId) + 1,
            OrderDate = input.OrderDate ?? DateOnly.FromDateTime(DateTime.Now),
            DeliveryDate = input.DeliveryDate ?? DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(7)),
            UserId = input.UserId,
            Code = code,
            Status = "Новый"
        };
        Dictionary<string, byte> articles = new();

        foreach (var article in input.Articles.Distinct())
            articles[article] = (byte)input.Articles.Count(a => a.Equals(article));

        foreach (var article in articles)
            order.OrderProducts.Add(new()
            {
                Article = article.Key,
                OrderId = order.OrderId,
                Quantity = article.Value
            });
        
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
        
        return order;
    }
    
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