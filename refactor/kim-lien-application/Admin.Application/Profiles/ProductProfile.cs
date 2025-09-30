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
                ;
            CreateMap<CreateProductDto, Product>()
                .ForMember(d => d.ProductCategories, 
                    opt => opt.MapFrom(s => s.CategoryIds.Select(x => new ProductCategory
                    {
                        CategoryId = x,
                    })))
                ;
        }
    }
}
