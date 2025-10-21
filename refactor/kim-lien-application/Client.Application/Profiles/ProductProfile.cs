using AutoMapper;
using Client.Application.Models.Product;
using Common.Domain.Entities;

namespace Client.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.ProductCategories.Select(x => x.Category)))
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.PictureAssets))
                ;
            CreateMap<Category, ProductCategoryDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Id))
                ;
        }
    }
}
