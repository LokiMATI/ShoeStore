using Microsoft.EntityFrameworkCore;
using ShoeShop.Library.Contexts;
using ShoeShop.Library.Models;

namespace ShoeShop.Library.Services;

public class ImageService(ShoeDbContext context)
{
    public async Task<Image> AddAsync(byte[] bytes)
    {
        Image image = new();
        image.ImageId = await context.Images.CountAsync() + 1;
        image.Bytes = bytes;
        
        context.Images.Add(image);
        await context.SaveChangesAsync();
        
        return image;
    }
}