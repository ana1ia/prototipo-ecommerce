namespace Ecommerce.Core.DTOs;

public class ProductSearchRequest
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public string? Keywords { get; set; }
} 