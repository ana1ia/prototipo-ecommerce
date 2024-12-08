using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly EcommerceDbContext _context;

    public ShoppingCartRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<ShoppingCart?> GetByUserIdAsync(string userId)
    {
        return await _context.ShoppingCarts
            .Include(sc => sc.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(sc => sc.UserId == userId);
    }

    public async Task<ShoppingCart> CreateCartAsync(string userId)
    {
        var cart = new ShoppingCart
        {
            UserId = userId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ShoppingCarts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task AddItemAsync(int cartId, int productId, int quantity)
    {
        var cart = await _context.ShoppingCarts
            .Include(sc => sc.Items)
            .FirstOrDefaultAsync(sc => sc.Id == cartId);

        if (cart == null)
            throw new ArgumentException("Cart not found");

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Items.Add(new ShoppingCartItem
            {
                ProductId = productId,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateItemQuantityAsync(int cartId, int productId, int quantity)
    {
        var item = await _context.ShoppingCartItems
            .FirstOrDefaultAsync(i => i.ShoppingCartId == cartId && i.ProductId == productId);

        if (item == null)
            throw new ArgumentException("Item not found in cart");

        item.Quantity = quantity;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(int cartId, int productId)
    {
        var item = await _context.ShoppingCartItems
            .FirstOrDefaultAsync(i => i.ShoppingCartId == cartId && i.ProductId == productId);

        if (item != null)
        {
            _context.ShoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(int cartId)
    {
        var items = await _context.ShoppingCartItems
            .Where(i => i.ShoppingCartId == cartId)
            .ToListAsync();

        _context.ShoppingCartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
    }
} 