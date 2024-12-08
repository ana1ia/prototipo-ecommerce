using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, 
                      opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ProductCount, 
                      opt => opt.MapFrom(src => src.Products.Count));

        CreateMap<ProductDto, Product>();
    }
} 