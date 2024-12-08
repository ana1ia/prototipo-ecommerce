using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly ShoppingCartService _cartService;

    public ShoppingCartController(ShoppingCartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ShoppingCart>> GetCart(string userId)
    {
        var cart = await _cartService.GetUserCart(userId);
        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPost("{userId}/items")]
    public async Task<IActionResult> AddToCart(string userId, [FromBody] AddToCartRequest request)
    {
        await _cartService.AddToCart(userId, request);
        return Ok();
    }

    [HttpPut("{cartId}/items/{productId}")]
    public async Task<IActionResult> UpdateCartItemQuantity(int cartId, int productId, [FromBody] int quantity)
    {
        await _cartService.UpdateCartItemQuantity(cartId, productId, quantity);
        return NoContent();
    }

    [HttpDelete("{cartId}/items/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int cartId, int productId)
    {
        await _cartService.RemoveFromCart(cartId, productId);
        return NoContent();
    }

    [HttpDelete("{cartId}")]
    public async Task<IActionResult> ClearCart(int cartId)
    {
        await _cartService.ClearCart(cartId);
        return NoContent();
    }
} 