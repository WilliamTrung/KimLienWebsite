using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Entities;

namespace AppService
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, DTOs.Category>().ReverseMap();
            CreateMap<Product, DTOs.Product>().ReverseMap();
            CreateMap<Role, DTOs.Role>().ReverseMap();
            CreateMap<User, DTOs.User>().ReverseMap();
        }
    }
}
