using ApiService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator
{
    public interface IRoleService
    {
        /// <summary>
        /// Get all roles
        /// <para>Throw KeyNotFoundException if no record in db</para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        IEnumerable<Role> GetRoles();
        Role GetRoleById(Guid Id);
        IEnumerable<Role> GetRolesByName(string Name);
        /// <summary>
        /// Add new role
        /// <para>Throw DuplicateNameException: role name duplicated</para>
        /// </summary>
        /// <param name="role"></param>
        /// <exception cref="DuplicateNameException">Duplicated role</exception>
        void Add(Role role);
        /// <summary>
        /// Update role name
        /// <para>Throw DuplicateNameException: role name duplicated</para>
        /// </summary>
        /// <param name="role"></param>
        /// <exception cref="DuplicateNameException">Duplicated role</exception>
        void Update(Role role);
        /// <summary>
        /// Delete a role
        /// <para>Throw KeyNotFoundException: No role with such Id found</para>
        /// </summary>
        /// <param name="roleId"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        void Delete(Guid roleId);
    }
}
