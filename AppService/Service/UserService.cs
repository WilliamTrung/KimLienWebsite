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
    public class UserService : BaseService<User, DTOs.User>, IUserService
    {
        public UserService(SqlContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<DTOs.User?> Login(DTOs.User user)
        {
            //User has no username or id = username? Ask tommorrow
            var find = await GetDTOs(filter: a => a.Id == user.Id && a.Password == user.Password);
            var found = find.FirstOrDefault();
            if (found != null)
            {
                return found;
            }
            return null;
        }

        public async Task<DTOs.User?> Register(DTOs.User user)
        {
            var find = await GetDTOs(filter: a => a.Id == user.Id && a.Password == user.Password);
            var found = find.FirstOrDefault();
            if (found == null)
            {
                var entity = await Create(user);
                if (entity != null)
                {
                    return _mapper.Map<DTOs.User>(entity);
                }
            }
            return null;
        }
        public override Task<DTOs.User> Create(DTOs.User dto)
        {
            //check duplicate email
            if (dto == null)
                return null;
            return base.Create(dto);
        }
    }
}
