using Admin.Application.Models.Product;
using AutoMapper;
using Common.Domain.Entities;

namespace Admin.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<ModifyProductDto, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.PictureAssets, opt => opt.MapFrom(s => s.Images))
                ;
            CreateMap<CreateProductDto, Product>()
                .ForMember(d => d.ProductCategories, 
                    opt => opt.MapFrom(s => s.CategoryIds.Select(x => new ProductCategory
                    {
                        CategoryId = x,
                    })))
                .ForMember(d => d.PictureAssets, opt => opt.MapFrom(s => s.Images))
                ;
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.Categories()))
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.PictureAssets))
                ;
            CreateMap<Category, ProductCategoryDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Id))
                ;
        }
    }
}
