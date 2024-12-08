using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetByUserIdAsync(string userId);
    Task<ShoppingCart> CreateCartAsync(string userId);
    Task AddItemAsync(int cartId, int productId, int quantity);
    Task UpdateItemQuantityAsync(int cartId, int productId, int quantity);
    Task RemoveItemAsync(int cartId, int productId);
    Task ClearCartAsync(int cartId);
} 