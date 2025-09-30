using Admin.Application.Models.Category;
using AutoMapper;
using Common.Domain.Entities;

namespace Admin.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<ModifyCategoryDto, Category>()
              .ForMember(d => d.Id, opt => opt.Ignore())
              ;
            CreateMap<CreateCategoryDto, Category>()
                ;
            CreateMap<Category, CategoryDto>()
                ;
        }
    }
}
