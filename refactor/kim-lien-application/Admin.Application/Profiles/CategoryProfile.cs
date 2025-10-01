using Admin.Application.Commands.Category;
using Admin.Application.Models.Category;
using AutoMapper;
using Common.Domain.Entities;

namespace Admin.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<ModifyCategoryCommand, Category>()
              .ForMember(d => d.Id, opt => opt.Ignore())
              ;
            CreateMap<CreateCategoryCommand, Category>()
                ;
            CreateMap<Category, CategoryDto>()
                ;
        }
    }
}
