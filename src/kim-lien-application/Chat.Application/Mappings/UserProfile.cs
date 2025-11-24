using AutoMapper;
using Chat.Application.Common.Models;
using Common.Domain.Entities;
using Common.Extension;
using Common.Kernel.Models.Implementations;

namespace Chat.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.Assets, opt =>
                {
                    opt.PreCondition(x => x.Asset != null);
                    opt.MapFrom(s => s.Asset!.TryDeserializeObject<List<AssetDto>>());
                })
                .ForMember(d => d.LastActiveAt, opt =>
                {
                    opt.PreCondition(x => x.UserMetadata != null);
                    opt.MapFrom(x => x.UserMetadata!.LastActiveAt);
                })
                .ForMember(d => d.IsOnline, opt =>
                {
                    opt.PreCondition(x => x.UserMetadata != null);
                    opt.MapFrom(x => x.UserMetadata!.IsOnline);
                })
                ;
        }
    }
}
