using Microsoft.EntityFrameworkCore;
using SoftbinatorProject.Core.Constants;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Services
{
    public interface ICourseService
    {
        public List<CourseInfo> GetCourses(string tags, string sort, string search, int page);
        public CompleteCourseInfo GetCompleteCourse(int courseId);
        public CourseInfo CreateCourse(CoursePost course, string creatorId);
        public CourseInfo AssignUserToCourse(int courseId, string userId);
        public CourseInfo RemoveUserFromCourse(int courseId, string userId);
        public CourseInfo UpdateCourse(int courseId, CoursePost course, string creatorId);
        public CourseInfo DeleteCourse(int courseId,string userId);

    }

    public class CourseService : ICourseService
    {
        private AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }


        public List<CourseInfo> GetCourses(string tags, string sort, string search, int page)
        {
            var sepTags = tags!=null?tags.Split(',').ToList():null;;

            var l = _context.Courses
                .Include(c => c.Tags)
                .Where(c => (sepTags == null || c.Tags.Where(t => sepTags.Contains(t.Name)).Any())
                && (search == null || c.Title.Contains(search) || c.Description.Contains(search)));

            switch (sort)
            {
                case "titleA":
                    l = l.OrderBy(c => c.Title);
                    break;
                case "titleD":
                    l = l.OrderByDescending(c => c.Title);
                    break;
                case "dateA":
                    l = l.OrderBy(c => c.CreatedAt);
                    break;
                case "dateB":
                    l = l.OrderByDescending(c => c.CreatedAt);
                    break;
                default:
                    break;
            }
            int nr = RandomConstants.CoursesPerPage;
            return l.Skip(page>0?(page - 1)*nr:0)
                .Take(nr)
                .Select(c => new CourseInfo(c))
                .ToList();
            
        }
        public CompleteCourseInfo GetCompleteCourse(int courseId)
        {
            if (_context.Courses.Find(courseId) == null)
            {
                return null;
            }
            return _context.Courses
                .Include(c => c.Creator)
                .Include(c => c.Users)
                .Include(c => c.Tags)
                .Include(c => c.Sections)
                .ThenInclude(s=>s.Chapters)
                .Where(c => c.Id == courseId)
                .Select(course => new CompleteCourseInfo(course))
                .First();
        }

        public CourseInfo CreateCourse(CoursePost course, string creatorId)
        {
            Course newCourse = new Course {
                Title = course.Title,
                Description = course.Description,
                CreatedAt = DateTime.Now,
                CreatorId = creatorId
            };
            _context.Courses.Add(newCourse);
            _context.SaveChanges();
            return new CourseInfo(newCourse);
        }

        public CourseInfo AssignUserToCourse(int courseId, string userId)
        {
            Course course = _context.Courses
                .Include(c => c.Users)
                .Include(c => c.Tags)
                .Where(course => course.Id == courseId)
                .FirstOrDefault();
            User user = _context.Users.Find(userId);
            if (course == null || course.Users.Contains(user) || userId == null)
            {
                return null;
            }
            course.Users.Add(user);
            _context.SaveChanges();
            return new CourseInfo(course);
        }

        public CourseInfo RemoveUserFromCourse(int courseId, string userId)
        {
            Course course = _context.Courses
                .Include(c => c.Users)
                .Include(c => c.Tags)
                .Where(course => course.Id == courseId)
                .FirstOrDefault();
            User user = _context.Users.Find(userId);
            if(course == null || !course.Users.Contains(user))
            {
                return null;
            }
            course.Users.Remove(user);
            _context.SaveChanges();
            return new CourseInfo(course);
        }

        public CourseInfo UpdateCourse(int courseId, CoursePost course, string creatorId)
        {
            Course courseToUpdate = _context.Courses.Find(courseId);
            if(courseToUpdate.CreatorId != creatorId)
            {
                return null;
            }
            if(course.Title != "")
            {
                courseToUpdate.Title = course.Title;
            }
            if(course.Description != "")
            {
                courseToUpdate.Description = course.Description;
            }
            _context.SaveChanges();
            return new CourseInfo(courseToUpdate);

        }

        public CourseInfo DeleteCourse(int courseId, string userId)
        {
            Course courseToDelete = _context.Courses.Find(courseId);
            if (courseToDelete == null || (userId != "Moderator" && courseToDelete.CreatorId != userId))
            {
                return null;
            }
            _context.Courses.Remove(courseToDelete);
            _context.SaveChanges();
            return new CourseInfo(courseToDelete);
        }



    }
}
