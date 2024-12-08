namespace Ecommerce.Core.Entities;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; } = null!;
    public ShoppingCart ShoppingCart { get; set; } = null!;
} 