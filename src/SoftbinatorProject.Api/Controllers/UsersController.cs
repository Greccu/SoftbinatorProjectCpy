using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using SoftbinatorProject.Api.Services;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User = SoftbinatorProject.Core.Models.User;

namespace SoftbinatorProject.Api.Controllers
{

    [SwaggerTag("Create a profile, Update it or see other people profiles.")]
    [Authorize(Roles = "User")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService,IRoleService roleService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        /// <summary>
        /// See all user profiles
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>

        [HttpGet]
        public IActionResult GetAll()
        {
            var ret = _userService.GetAllUsers();
            return Ok(ret);
        }

        /// <summary>
        /// See your own profile
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet]
        public IActionResult GetOwn()
        {
            var ret = _userService.GetUserInfo(CurrentUserId());
            if(ret == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(ret);
        }

        /// <summary>
        /// See a specific user profile
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            UserInfo user = _userService.GetUserInfo(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Register and create your profile
        /// </summary>
        
        [Authorize]
        [HttpPost]
        public IActionResult Register([FromBody] UserPost user)
        {
            var Id = CurrentUserId();
            var Email = User.FindFirstValue("emails");
            UserInfo newUser = new UserInfo(user, Id, Email);
            UserInfo ret = _userService.CreateUserProfile(newUser);
            if(ret == null)
            {
                return BadRequest("Something went wrong");
            }
            _roleService.AssignToRole(Id, 4);
            return Ok(ret);

        }

        /// <summary>
        /// Update your profile
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpPut]
        public IActionResult Put([FromBody] UserPost user){
            var Id = CurrentUserId();
            var ret = _userService.EditUserProfile(Id, user);
            if(ret == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [NonAction]
        private string CurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
