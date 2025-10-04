using Authen.Application.Commands;
using Authen.Application.Models;
using AutoMapper;

namespace Authen.Application.Profiles
{
    public class AuthenProfile : Profile
    {
        public AuthenProfile()
        {
            CreateMap<RegisterDto, RegisterCommand>();
        }
    }
}
