using AppCore;
using AppCore.Entities;
using AppService.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Service
{
    public class RoleService : BaseService<Role, DTOs.Role>, IRoleService
    {
        public RoleService(SqlContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
