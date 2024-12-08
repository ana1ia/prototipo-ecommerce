using Ecommerce.Core.DTOs;
using Ecommerce.Core.Interfaces;
using AutoMapper;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Services;

public class ProductService
{ 
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> SearchProducts(ProductSearchRequest request)
    {
        var products =  await _productRepository.SearchProductsAsync(
            request.SearchTerm,
            request.CategoryId,
            request.Keywords);

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProduct(ProductDto product)
    {
        var createdProduct = await _productRepository.AddAsync(_mapper.Map<Product>(product));
        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task UpdateProduct(ProductDto product)
    {
        await _productRepository.UpdateAsync(_mapper.Map<Product>(product));
    }
} 