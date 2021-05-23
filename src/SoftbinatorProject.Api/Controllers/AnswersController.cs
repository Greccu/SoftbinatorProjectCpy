using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftbinatorProject.Api.Services;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Controllers
{
    [SwaggerTag("View answers or answer to a question.")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IUserCourseService _userCourseService;

        public AnswersController(IAnswerService answerService, IUserCourseService userCourseService)
        {
            _answerService = answerService;
            _userCourseService = userCourseService;
        }

        /// <summary>
        /// See all answers to a question.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{questionId}")]
        public IActionResult GetAll(int questionId)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourseQuestion(CurrentUserId(), questionId))
            {
                return Forbid();
            }
            var ret = _answerService.GetAnswers(questionId);
            if(ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// See an answer.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || _userCourseService.UserJoinedCourseAnswer(CurrentUserId(), id))
            {
                return Forbid();
            }
            var ret = _answerService.GetAnswer(id);
            if (ret == null)
            {
                return NotFound();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Answer to a question.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpPost("{questionId}")]
        public IActionResult Post(int questionId, [FromBody] QnAPost question)
        {
            var ret = _answerService.CreateAnswer(questionId, question, CurrentUserId());
            if(ret == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(ret);
        }

        /// <summary>
        /// Update an answer.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] QnAPost question)
        {
            var ret = _answerService.UpdateAnswer(id, question, CurrentUserId());
            if (ret == null)
            {
                return Forbid();
            }
            return Ok(ret);
        }

        /// <summary>
        /// Delete an answer.
        /// </summary>
        /// <remarks>
        /// You need to be registered to access this resource.
        /// </remarks>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = CurrentUserId();
            var ret = _answerService.DeleteAnswer(id, userId);
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
