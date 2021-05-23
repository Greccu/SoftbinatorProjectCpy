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
    [SwaggerTag("Assign a tag to your course.")]
    [Authorize(Roles = "User")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }


        /// <summary>
        /// Assign a tag to your course.
        /// </summary>
        /// <remarks>
        /// You need to be a creator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator")]
        [HttpPost("{courseId}/{tagId}")]
        public IActionResult Assign(int courseId, int tagId)
        {
            var userId = CurrentUserId();
            var ret = _tagService.AssignTag(courseId, tagId, userId);
            
            if (ret == null)
            {
                return Forbid();
            }
            return Ok(ret);
        }


        /// <summary>
        /// See all the tags.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet]
        public IActionResult GetAll()
        {
            var ret = _tagService.GetAllTags();
            return Ok(ret);
        }

        /// <summary>
        /// See a tag.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var ret = _tagService.GetTag(id);
            if(ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Create a tag.
        /// </summary>
        /// <remarks>
        /// You need to be a moderator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public IActionResult Post([FromBody] TagPost tag)
        {
            var ret = _tagService.CreateTag(tag);
            if (ret == null)
            {
                return BadRequest("An error occured");
            }
            return Ok(ret);
        }

        /// <summary>
        /// Update a tag.
        /// </summary>
        /// <remarks>
        /// You need to be a moderator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Moderator")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TagPost tag)
        {
            var ret = _tagService.UpdateTag(id, tag);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok();
        }
        /// <summary>
        /// Delete a tag.
        /// </summary>
        /// <remarks>
        /// You need to be a moderator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Moderator")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ret = _tagService.DeleteTag(id);
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
