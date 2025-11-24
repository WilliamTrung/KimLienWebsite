using AutoMapper;
using Common.Application.Storage.Models;
using Common.Infrastructure.Storage.Azure.Commands;

namespace Common.Infrastructure.Storage.Azure.Profiles
{
    public class AzureStorageProfile : Profile
    {
        public AzureStorageProfile()
        {
            CreateMap<FileUpload, AzureUploadFileCommand>().ReverseMap();
        }
    }
}
