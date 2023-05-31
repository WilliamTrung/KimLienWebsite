using ApiService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator
{
    public interface IUserService
    {
        IEnumerable<User>? GetUsers();
        /// <summary>
        /// Find user by Id        
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// <para>User</para>
        /// <para>Null if not found</para>
        /// </returns>
        User? GetUserById(Guid Id);
        /// <summary>
        /// Find users by role name        
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>
        /// <para>List users</para>
        /// <para>Null if not found</para>
        /// </returns>
        IEnumerable<User>? GetUsersByRole(string role);
        void Add(User user);
        void Update(User user);
        /// <summary>
        /// Not supported function
        /// <br/>Contact the dev team
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="NotSupportedException"></exception>
        void Delete(Guid userId);
    }
}
