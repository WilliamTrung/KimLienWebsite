using ApiService.Azure;
using ApiService.DTOs;
using ApiService.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator.Implementation
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService) : base(unitOfWork, mapper, azureService)
        {
        }
        /// <summary>
        /// Add new role
        /// <para>Throw on role name duplicated</para>
        /// </summary>
        /// <param name="role"></param>
        /// <exception cref="DuplicateWaitObjectException">Duplicated role</exception>
        public void Add(Role role)
        {
            using(var roleRepos = _unitOfWork.RoleRepository)
            {
                role.Name = role.Name.Trim();
                var roles = roleRepos.Get();
                if (roles.Any(r => Extension.StringExtension.MinimalCompareString(role.Name, r.Name)))                   
                {
                    throw new DuplicateNameException();
                }
                roleRepos.Create(_mapper.Map<AppCore.Entities.Role>(role));
                _unitOfWork.Save();
            }
        }
        /// <summary>
        /// Delete a role
        /// <para>Throw KeyNotFoundException: No role with such Id found</para>
        /// </summary>
        /// <param name="roleId"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Delete(Guid roleId)
        {
            using (var roleRepos = _unitOfWork.RoleRepository)
            {
                var role = roleRepos.GetById(roleId);
                if(role == null) 
                    throw new KeyNotFoundException();
                roleRepos.Delete(role);                
                _unitOfWork.Save();
            }
        }
        /// <summary>
        /// Find role by Id
        /// <para>Throw if not found</para>
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Role GetRoleById(Guid Id)
        {
            using (var roleRepos = _unitOfWork.RoleRepository)
            {
                var result = roleRepos.GetById(Id);                
                if(result != null)
                {
                    return _mapper.Map<Role>(result);
                } else
                {
                    throw new KeyNotFoundException();
                }
            }
        }
        /// <summary>
        /// Find roles by name
        /// <para>Throw KeyNotFoundException if no role has been found</para>
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public IEnumerable<Role> GetRolesByName(string Name)
        {
            using (var roleRepos = _unitOfWork.RoleRepository)
            {
                var roles = roleRepos.Get();
                var result = roles.Where(r => Extension.StringExtension.MinimalCompareString(Name, r.Name));
                if (result != null)
                {
                    return _mapper.Map<IEnumerable<Role>>(result);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }
        /// <summary>
        /// Get all roles
        /// <para>Throw KeyNotFoundException if no record in db</para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public IEnumerable<Role> GetRoles()
        {
            using (var roleRepos = _unitOfWork.RoleRepository)
            {
                var result = roleRepos.Get();
                if (result != null)
                {
                    return _mapper.Map<IEnumerable<Role>>(result);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public void Update(Role role)
        {
            using (var roleRepos = _unitOfWork.RoleRepository)
            {
                role.Name = role.Name.Trim();
                var roles = roleRepos.Get();
                if (roles.Any(r => Extension.StringExtension.MinimalCompareString(role.Name, r.Name)))
                {
                    throw new DuplicateNameException();
                }
                roleRepos.Update(_mapper.Map<AppCore.Entities.Role>(role));
                _unitOfWork.Save();
            }
        }
    }
}
