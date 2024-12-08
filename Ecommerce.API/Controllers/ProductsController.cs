using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Ecommerce.API.Controllers;

/// <summary>
/// Controller for managing product operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public ProductsController(ProductService productService, ILogger<ProductsController> logger, IMemoryCache cache)
    {
        _productService = productService;
        _logger = logger;
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
    }

    /// <summary>
    /// Searches for products based on specified criteria
    /// </summary>
    /// <param name="request">Search parameters</param>
    /// <returns>List of products matching the search criteria</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] ProductSearchRequest request)
    {
        try
        {
            var cacheKey = $"SearchProducts_{request.SearchTerm ?? "null"}_" +
            $"{request.CategoryId?.ToString() ?? "null"}_" +
            $"{request.Keywords ?? "null"}";

            var cachedProducts = _cache.Get<IEnumerable<ProductDto>>(cacheKey);

            if (cachedProducts != null)
                return Ok(new { size = cachedProducts.Count(), products = cachedProducts });

            var products = await _productService.SearchProducts(request);
            _cache.Set(cacheKey, products, _cacheOptions);

            return Ok(new { size = products.Count(), products });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching products");
            return StatusCode(500, "An error occurred while processing request");
        }

    }

    /// <summary>
    /// Gets a specific product by ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <returns>The requested product</returns>
    /// <response code="200">Returns the requested product</response>
    /// <response code="404">If the product is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting product ");
            return StatusCode(500, "An error occurred while processing request");
        }
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="product">The product information to create</param>
    /// <returns>The created product</returns>
    /// <response code="201">Returns the newly created product</response>
    /// <response code="400">If the product data is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto product)
    {
        try
        {
            var createdProduct = await _productService.CreateProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating product");
            return StatusCode(500, "An error occurred while processing request");
        }
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">The ID of the product to update</param>
    /// <param name="product">The updated product information</param>
    /// <returns>No content</returns>
    /// <response code="204">If the product was successfully updated</response>
    /// <response code="400">If the ID doesn't match the product ID</response>
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateProduct(int id, ProductDto product)
    {

        try
        {
            if (id != product.Id)
                return BadRequest();

            await _productService.UpdateProduct(product);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating product");
            return StatusCode(500, "An error occurred while processing request");
        }
    }
} 