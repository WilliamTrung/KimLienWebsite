using AutoMapper;
using Client.Application.Models.Category;
using Common.Domain.Entities;

namespace Client.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.PictureAssets))
                ;

            CreateMap<Category, CategoryTreeDto>()
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.PictureAssets))
                .ForMember(d => d.ProductCount, opt => opt.MapFrom(s => s.ProductCategories.Count))
                .ForMember(d => d.ChildCategories, opt => opt.Ignore())
                ;
        }
    }
}
