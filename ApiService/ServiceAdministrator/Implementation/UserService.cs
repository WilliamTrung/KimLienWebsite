using ApiService.Azure;
using ApiService.DTOs;
using ApiService.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator.Implementation
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService) : base(unitOfWork, mapper, azureService)
        {
        }
        public void Add(CreateAccountModel user)
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                if(String.IsNullOrEmpty(user.Password))
                {
                    throw new ArgumentException("Password is null");
                }
                string password = user.Password;
                Extension.HashExtension.Hash(ref password);
                user.Password = password;
                userRepos.Create(_mapper.Map<AppCore.Entities.User>(user));
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
                return _mapper.Map<User>(result);
            }
        }

        public IEnumerable<User>? GetUsers()
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                var result = userRepos.Get();
                return _mapper.Map<IEnumerable<User>>(result);
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var result = userRepos.Get(filter: u => Extension.StringExtension.MinimalCompareString(role, u.Role.Name), includeProperties: "Role");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                return _mapper.Map<IEnumerable<User>>(result);
            }
        }

        public User? Login(Guid id, string pwdHashed)
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                var findById = userRepos.Get(u => u.Id == id, includeProperties: "Role").FirstOrDefault();
                if(findById != null)
                {
                    if (findById.Password == pwdHashed)
                    {
                        return _mapper.Map<User>(findById);
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Throw ArgumentException: Password is null or empty
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Update(User user)
        {
            using (var userRepos = _unitOfWork.UserRepository)
            {
                if (String.IsNullOrEmpty(user.Password))
                {
                    throw new ArgumentException("Password is null");
                }
                userRepos.Update(_mapper.Map<AppCore.Entities.User>(user));
                _unitOfWork.Save();
            }
        }
    }
}
