namespace Ecommerce.Core.Entities;

public class ShoppingCart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
} 