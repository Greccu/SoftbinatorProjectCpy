using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftbinatorProject.Api.Services;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoftbinatorProject.Api.Controllers
{
    [SwaggerTag("Manage the platform's roles.")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {

        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Assign a user to a role.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPost("{userId}/{roleId}")]
        public IActionResult Assign(string userId, int roleId)
        {
            var ret = _roleService.AssignToRole(userId, roleId);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }


        /// <summary>
        /// See all roles.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpGet]
        public IActionResult GetAll()
        {
            var ret = _roleService.GetAllRoles();
            return Ok(ret);
        }

        /// <summary>
        /// See own roles.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles ="User")]
        public IActionResult GetOwn()
        {
            return Ok(_roleService.GetUserRoles(CurrentUserId()));
        }

        /// <summary>
        /// See a user's roles.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var ret = _roleService.GetUserRoles(id);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// See all users in a role.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpGet("{roleName}")]
        public IActionResult GetUsers(string roleName)
        {
            var ret = _roleService.GetUsersInRole(roleName);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Update a role.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpPost]
        public IActionResult Post([FromBody] RolePost role)
        {
            var ret = _roleService.CreateRole(role);
            if(ret == null)
            {
                return BadRequest("There already exists a role with that name");
            }
            return Ok(ret);
  
        }

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ret = _roleService.DeleteRole(id);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }


        [NonAction]
        private string CurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
