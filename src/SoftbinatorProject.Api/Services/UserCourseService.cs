using Microsoft.EntityFrameworkCore;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Services
{
    public interface IUserCourseService
    {
        public bool UserJoinedCourse(string userId, int courseId);
        public bool UserJoinedCourseSection(string userId, int sectionId);
        public bool UserJoinedCourseChapter(string userId, int chapterId);
        public bool UserJoinedCourseQuestion(string userId, int questionId);
        public bool UserJoinedCourseAnswer(string userId, int answerId);
    }
    public class UserCourseService : IUserCourseService
    {
        private readonly AppDbContext _context;
        public UserCourseService(AppDbContext context)
        {
            _context = context;
        }

        public bool UserJoinedCourse(string userId, int courseId)
        {
            User user = _context.Users
                .Include(u => u.Courses)
                .FirstOrDefault();
            Course course = _context.Courses
                .Find(courseId);
            return (user != null && course != null && user.Courses.Contains(course));

        }

        public bool UserJoinedCourseSection(string userId, int sectionId)
        {
            Section section = _context.Sections.Find(sectionId);
            if (section == null)
            {
                return false;
            }
            return UserJoinedCourse(userId, section.CourseId);
        }

        public bool UserJoinedCourseChapter(string userId, int chapterId)
        {
            Chapter chapter = _context.Chapters.Find(chapterId);
            if (chapter == null)
            {
                return false;
            }
            return UserJoinedCourseSection(userId, chapter.SectionId);
        }

        public bool UserJoinedCourseQuestion(string userId, int questionId)
        {
            Question question = _context.Questions.Find(questionId);
            if (question == null)
            {
                return false;
            }
            return UserJoinedCourseChapter(userId, question.ChapterId);
        }
        public bool UserJoinedCourseAnswer(string userId, int answerId)
        {
            Answer answer = _context.Answers.Find(answerId);
            if (answer == null)
            {
                return false;
            }
            return UserJoinedCourseQuestion(userId, answer.QuestionId);
        }
    }
}
