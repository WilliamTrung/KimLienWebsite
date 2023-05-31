using ApiService.Azure;
using ApiService.DTOs;
using ApiService.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator.Implementation
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService) : base(unitOfWork, mapper, azureService)
        {
        }

        public void Add(User user)
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                userRepos.Create(user);
                _unitOfWork.Save();
            }
        }
        /// <summary>
        /// Not supported function
        /// <br/>Contact the dev team
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="NotSupportedException"></exception>
        public void Delete(Guid userId)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Find user by Id        
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// <para>User</para>
        /// <para>Null if not found</para>
        /// </returns>
        public User? GetUserById(Guid Id)
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                var result = userRepos.GetById(Id);
                return result;
            }
        }

        public IEnumerable<User>? GetUsers()
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                var result = userRepos.Get();
                return result;
            }
        }
        /// <summary>
        /// Find users by role name        
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>
        /// <para>List users</para>
        /// <para>Null if not found</para>
        /// </returns>
        public IEnumerable<User>? GetUsersByRole(string role)
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                var result = userRepos.Get(filter: u => Extension.StringExtension.MinimalCompareString(role, u.Role.Name), includeProperties: "Role");
                return result;
            }
        }

        public void Update(User user)
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                userRepos.Update(user);
                _unitOfWork.Save();
            }
        }
    }
}
