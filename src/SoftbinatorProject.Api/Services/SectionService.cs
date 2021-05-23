using Microsoft.EntityFrameworkCore;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Services
{
    public interface ISectionService
    {
        public List<SectionInfo> GetCourseSections(int courseId);
        public SectionInfo GetSection(int sectionId);
        public SectionInfo CreateSection(SectionPost section, string userId);
        public SectionInfo EditSection(int sectionId, SectionPost section, string userId);
        public SectionInfo DeleteSection(int sectionId, string userId);
    }
    public class SectionService : ISectionService
    {
        private readonly AppDbContext _context;
        public SectionService(AppDbContext context)
        {
            _context = context;
        }

        public List<SectionInfo> GetCourseSections(int courseId)
        {
            if(_context.Courses.Find(courseId) == null)
            {
                return null;
            }
            return _context.Sections
                .Include(s => s.Chapters)
                .Where(s => s.CourseId == courseId)
                .Select(s => new SectionInfo(s))
                .ToList();
        }

        public SectionInfo GetSection(int sectionId)
        {
            var section = _context.Sections
                .Include(s => s.Chapters)
                .Where(s => s.Id == sectionId)
                .FirstOrDefault();
            if (section == null)
            {
                return null;
            }
            return new SectionInfo(section);
        }
        public SectionInfo CreateSection(SectionPost section, string userId)
        {
            Course course = _context.Courses
                .Find(section.CourseId);
            if(course == null || course.CreatorId != userId)
            {
                return null;
            }
            Section newSection = new Section()
            {
                Title = section.Title,
                Description = section.Description,
                Number = section.Number,
                CourseId = section.CourseId
            };
            _context.Sections.Add(newSection);
            _context.SaveChanges();
            return new SectionInfo(newSection);
        }
        public SectionInfo EditSection(int sectionId, SectionPost section, string userId) {
            Section sectionToEdit = _context.Sections
                .Include(s => s.Course)
                .Where(sc => sc.Id == sectionId)
                .FirstOrDefault();

            if(sectionToEdit == null || sectionToEdit.Course.CreatorId != userId)
            {
                return null;
            }
            sectionToEdit.Title = section.Title;
            sectionToEdit.Description = section.Description;
            sectionToEdit.Number = section.Number;
            sectionToEdit.CourseId = section.CourseId;
            _context.SaveChanges();
            return new SectionInfo(sectionToEdit);
        }
        public SectionInfo DeleteSection(int sectionId, string userId)
        {
            Section sectionToDelete = _context.Sections
                .Include(s => s.Course)
                .Where(sc => sc.Id == sectionId)
                .FirstOrDefault();
            if (sectionToDelete == null || (userId != "Moderator" && sectionToDelete.Course.CreatorId != userId))
            {
                return null;
            }
            _context.Sections.Remove(sectionToDelete);
            return new SectionInfo(sectionToDelete);
        }
    }
}
