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
    [SwaggerTag("See sections, create a new one or update it.")]
    [Authorize(Roles="User")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        private readonly IUserCourseService _userCourseService;

        public SectionsController(ISectionService sectionService, IUserCourseService userCourseService)
        {
            _sectionService = sectionService;
            _userCourseService = userCourseService;
        }

        /// <summary>
        /// See all sections from a course.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{courseId}")]
        public IActionResult GetAll(int courseId)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourse(CurrentUserId(), courseId))
            {
                return Forbid();
            }
            var ret = _sectionService.GetCourseSections(courseId);
            if(ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// See a section.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{sectionId}")]
        public IActionResult Get(int sectionId)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourseSection(CurrentUserId(), sectionId))
            {
                return Forbid();
            }
            var ret = _sectionService.GetSection(sectionId);
            if(ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Create a section for your course.
        /// </summary>
        /// <remarks>
        /// You need to be a creator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator")]
        [HttpPost]
        public IActionResult Post([FromBody] SectionPost section)
        {
            string userId = CurrentUserId();
            var ret = _sectionService.CreateSection(section, userId);
            if(ret == null)
            {
                return Forbid();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Update a sections from your course.
        /// </summary>
        /// <remarks>
        /// You need to be a creator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SectionPost section)
        {
            string userId = CurrentUserId();
            var ret = _sectionService.EditSection(id, section,userId);
            if(ret == null)
            {
                return Forbid();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Delete a section from your course.
        /// </summary>
        /// <remarks>
        /// You need to be a creator or a moderator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator,Moderator")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string userId = User.IsInRole("Moderator") ? "Moderator" : CurrentUserId();
            var ret = _sectionService.DeleteSection(id,userId);
            if (ret == null)
            {
                return Forbid();
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
