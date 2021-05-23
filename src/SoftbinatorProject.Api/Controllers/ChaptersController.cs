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
    [SwaggerTag("See chapters, create a new one or update it.")]
    [Authorize(Roles = "User")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterService _chapterService;
        private readonly IUserCourseService _userCourseService;

        public ChaptersController(IChapterService sectionService, IUserCourseService userCourseService)
        {
            _chapterService = sectionService;
            _userCourseService = userCourseService;
        }

        /// <summary>
        /// See all chapters from a section.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{sectionId}")]
        public IActionResult GetAll(int sectionId)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourseSection(CurrentUserId(), sectionId))
            {
                return Forbid();
            }
            var ret = _chapterService.GetChapters(sectionId);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// See a chapter.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{chapterId}")]
        public IActionResult Get(int chapterId)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourseChapter(CurrentUserId(), chapterId))
            {
                return Forbid();
            }
            var ret = _chapterService.GetChapter(chapterId);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Create a chapter to your course's section.
        /// </summary>
        /// <remarks>
        /// You need to be a creator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator")]
        [HttpPost]
        public IActionResult Post([FromBody] ChapterPost chapter)
        {
            string userId = CurrentUserId();
            var ret = _chapterService.CreateChapter(chapter,userId);
            if (ret == null)
            {
                return Forbid();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Update a chapter from your course's section.
        /// </summary>
        /// <remarks>
        /// You need to be a creator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ChapterPost chapter)
        {
            string userId = CurrentUserId();
            var ret = _chapterService.EditChapter(id, chapter,userId);
            if (ret == null)
            {
                return Forbid();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Delete a chapter from your course's section.
        /// </summary>
        /// <remarks>
        /// You need to be a creator or a moderator to access this resource.
        /// </remarks>
        [Authorize(Roles = "Creator,Moderator")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string userId = User.IsInRole("Moderator") ? "Moderator" : CurrentUserId();
            var ret = _chapterService.DeleteChapter(id,userId);
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
