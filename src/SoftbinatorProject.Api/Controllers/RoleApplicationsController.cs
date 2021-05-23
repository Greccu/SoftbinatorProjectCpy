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
    [SwaggerTag("Apply for a better role on the platform.")]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleApplicationsController : ControllerBase
    {
        private readonly IRoleApplicationService _roleApplicationService;
        public RoleApplicationsController(IRoleApplicationService roleApplicationService)
        {
            _roleApplicationService = roleApplicationService;
        }

        /// <summary>
        /// Accept a role application.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpPost]
        public IActionResult Accept(int id)
        {
            var ret = _roleApplicationService.ChangeStatus(id, "Accepted");
            if(ret == null)
            {
                return BadRequest("An error occured");
            }
            return Ok(ret);
        }

        /// <summary>
        /// Decline a role application.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpPost]
        public IActionResult Decline(int id)
        {
            var ret = _roleApplicationService.ChangeStatus(id, "Declined");
            if (ret == null)
            {
                return BadRequest("An error occured");
            }
            return Ok(ret);
        }

        /// <summary>
        /// See all role applications.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpGet]
        public IActionResult Get()
        {
            var ret = _roleApplicationService.GetRoleApplications("*");
            return Ok(ret);
        }

        /// <summary>
        /// See all active role applications.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpGet]
        public IActionResult GetActive()
        {
            var ret = _roleApplicationService.GetRoleApplications("Created");
            return Ok(ret);
        }

        /// <summary>
        /// See a role application.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var ret = _roleApplicationService.GetRoleApplication(id);
            if(ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Create a role application.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Post([FromBody] RoleApplicationPost ra)
        {
            var userId = CurrentUserId();
            var ret = _roleApplicationService.CreateRoleApplication(userId,ra);
            if(ret == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(ret);
        }

        /// <summary>
        /// Delete a role application.
        /// </summary>
        /// <remarks>
        /// You need to an admin to access this resource.
        /// </remarks>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ret = _roleApplicationService.DeleteRoleApplication(id);
            if (ret == null)
            {
                return BadRequest("Something went wrong");
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
