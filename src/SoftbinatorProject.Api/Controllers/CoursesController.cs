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
    [SwaggerTag("See the platform's courses or create your own one.")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CoursesController : ControllerBase
    {

        private readonly ICourseService _courseService;
        private readonly IUserCourseService _userCourseService;

        public CoursesController(ICourseService courseService, IUserCourseService userCourseService)
        {
            _courseService = courseService;
            _userCourseService = userCourseService;
        }

        /// <summary>
        /// Join a course to have access to it's resources.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpPost("{id}")]
        public IActionResult Join(int id)
        {
            var ret = _courseService.AssignUserToCourse(id, CurrentUserId());
            if (ret == null)
            {
                return BadRequest("You have already joined this course or the course does not exists");
            }
            return Ok(ret);
        }

        /// <summary>
        /// Leave a course.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpPost("{id}")]
        public IActionResult Leave(int id)
        {
            var ret = _courseService.RemoveUserFromCourse(id, CurrentUserId());
            if (ret == null)
            {
                return BadRequest("You are not part of this course or the course does not exists");
            }
            return Ok();
        }

        /// <summary>
        /// See all courses.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource. <br/>
        /// You can filter courses by tags and sort them by different filters. <br/>
        /// The available filters are : titleA, titleD, dateA, dateD.
        /// </remarks>
        [HttpGet]
        public IActionResult GetAll([FromQuery] string tags, string sort, string search, int page)
        {
            var ret = _courseService.GetCourses(tags,sort,search,page);
            return Ok(ret);
        }

        /// <summary>
        /// See a course.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if(User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourse(CurrentUserId(), id))
            {
                return Forbid();
            }
            var ret = _courseService.GetCompleteCourse(id);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Create a new course.
        /// </summary>
        /// <remarks>
        /// You need to be a creator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator")]
        [HttpPost]
        public IActionResult Post([FromBody] CoursePost course)
        {
            var ret = _courseService.CreateCourse(course, CurrentUserId());
            if (ret == null)
            {
                return BadRequest("An error occureed");
            }
            return Ok(ret);
        }


        /// <summary>
        /// Update a course.
        /// </summary>
        /// <remarks>
        /// You need to be a creator to access this resource.
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        public IActionResult Put(int id, [FromBody] CoursePost course)
        {

            CoursePost ret = _courseService.UpdateCourse(id, course, CurrentUserId());
            if(ret == null)
            {
                return Forbid();
            }
            return Ok();
        }

        /// <summary>
        /// Delete a course.
        /// </summary>
        /// <remarks>
        /// You need to be a creator or a moderator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator,Moderator")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string userId = User.IsInRole("Moderator") ? "Moderator" : CurrentUserId();
            var ret = _courseService.DeleteCourse(id,userId);
            if(ret == null)
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
