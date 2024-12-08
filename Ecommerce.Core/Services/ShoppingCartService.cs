using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Core.Services;

public class ShoppingCartService
{
    private readonly IShoppingCartRepository _cartRepository;

    public ShoppingCartService(IShoppingCartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<ShoppingCart?> GetUserCart(string userId)
    {
        return await _cartRepository.GetByUserIdAsync(userId);
    }

    public async Task AddToCart(string userId, AddToCartRequest request)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            cart = await _cartRepository.CreateCartAsync(userId);
        }

        await _cartRepository.AddItemAsync(cart.Id, request.ProductId, request.Quantity);
    }

    public async Task UpdateCartItemQuantity(int cartId, int productId, int quantity)
    {
        await _cartRepository.UpdateItemQuantityAsync(cartId, productId, quantity);
    }

    public async Task RemoveFromCart(int cartId, int productId)
    {
        await _cartRepository.RemoveItemAsync(cartId, productId);
    }

    public async Task ClearCart(int cartId)
    {
        await _cartRepository.ClearCartAsync(cartId);
    }
} 