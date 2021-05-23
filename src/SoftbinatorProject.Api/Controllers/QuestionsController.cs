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
    [SwaggerTag("View questions or ask a question")]
    [Authorize(Roles = "User")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IUserCourseService _userCourseService;

        public QuestionsController(IQuestionService questionService, IUserCourseService userCourseService)
        {
            _questionService = questionService;
            _userCourseService = userCourseService;
        }


        /// <summary>
        /// See all questions from a chapter.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>

        [HttpGet("{chapterId}")]
        public IActionResult GetAll(int chapterId,[FromQuery] string sort, int page)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourseChapter(CurrentUserId(), chapterId))
            {
                return Forbid();
            }
            var ret = _questionService.GetQuestions(chapterId,sort,page);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }


        /// <summary>
        /// See a question.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourseQuestion(CurrentUserId(), id))
            {
                return Forbid();
            }
            var ret = _questionService.GetQuestion(id);
            if(ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Ask a question.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpPost("{chapterId}")]
        public IActionResult Post(int chapterId,[FromBody] QnAPost question)
        {
            var userId = CurrentUserId();
            var ret = _questionService.CreateQuestion(chapterId, question, userId);
            if(ret == null)
            {
                return BadRequest();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Update a question.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] QnAPost question)
        {
            var userId = CurrentUserId();
            var ret = _questionService.UpdateQuestion(id, question, userId);
            if (ret == null)
            {
                return Forbid();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Delete a question.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string userId = User.IsInRole("Moderator") ? "Moderator" : CurrentUserId();
            var ret = _questionService.DeleteQuestion(id,userId);
            if(ret == null)
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
