using ApiService.DTOs;
using ApiService.ServiceAdministrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.Administrator
{
    [Route("api/administrator/users")]
    [Authorize(Roles = "Administrator")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService= userService;
        }

        // GET: api/<RoleController>
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var result = _userService.GetUsers();
            return Ok(result);
        }

        // POST api/<RoleController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateAccountModel user)
        {
            try
            {
                _userService.Add(user);
            }
            catch (DuplicateNameException)
            {
                return Ok(StatusCode(StatusCodes.Status409Conflict, "Duplicated name"));
            }
            return Ok(StatusCode(StatusCodes.Status202Accepted));
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] User user)
        {
            try
            {
                _userService.Update(user);
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
                _userService.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                return Ok(StatusCode(StatusCodes.Status404NotFound, "Role not found"));
            }
            return Ok(StatusCode(StatusCodes.Status202Accepted));
        }
    }
}
