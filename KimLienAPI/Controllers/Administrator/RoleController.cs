using ApiService.DTOs;
using ApiService.ServiceAdministrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.Administrator
{
    [Route("api/administrator/roles")]
    [Authorize(Roles = "Administrator")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/<RoleController>
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
        }

        // POST api/<RoleController>
        [HttpPost]
        public IActionResult Post([FromBody] Role role)
        {
            try
            {
                _roleService.Add(role);
            }
            catch (DuplicateNameException)
            {
                return Ok(StatusCode(StatusCodes.Status409Conflict, "Duplicated name"));
            }
            return Ok(StatusCode(StatusCodes.Status202Accepted));
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Role role)
        {
            try
            {
                _roleService.Update(role);
            }
            catch (DuplicateNameException)
            {
                return Ok(StatusCode(StatusCodes.Status409Conflict, "Duplicated name"));
            }
            return Ok(StatusCode(StatusCodes.Status202Accepted));
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _roleService.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                return Ok(StatusCode(StatusCodes.Status404NotFound, "Role not found"));
            }
            return Ok(StatusCode(StatusCodes.Status202Accepted));
        }
    }
}
