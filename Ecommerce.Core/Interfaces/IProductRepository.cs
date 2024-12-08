using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> SearchProductsAsync(string? searchTerm, int? categoryId, string? keywords);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
} 