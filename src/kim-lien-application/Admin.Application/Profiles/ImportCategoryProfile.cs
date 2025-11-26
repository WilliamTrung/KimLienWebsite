using Admin.Contract.Commands;
using AutoMapper;
using Common.Domain.Entities;

namespace Admin.Application.Profiles
{
    public class ImportCategoryProfile : Profile
    {
        public ImportCategoryProfile()
        {
            CreateMap<CreateCategoryContractCommand, Category>()
                ;
        }
    }
}
