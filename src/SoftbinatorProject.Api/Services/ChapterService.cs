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
    public interface IChapterService
    {
        
        public List<ChapterInfo> GetChapters(int sectionId);
        public ChapterInfo GetChapter(int chapterId);
        public ChapterInfo CreateChapter(ChapterPost chapter, string userId);
        public ChapterInfo EditChapter(int chapterId, ChapterPost chapter, string userId);
        public ChapterInfo DeleteChapter(int chapterId, string userId);
        
    }
    public class ChapterService : IChapterService
    {
        private readonly AppDbContext _context;

        public ChapterService(AppDbContext context)
        {
            _context = context;
        }
        
        public List<ChapterInfo> GetChapters(int sectionId)
        {
            if(_context.Sections.Find(sectionId) == null)
            {
                return null;
            }
            return _context.Chapters
                .Where(s => s.SectionId == sectionId)
                .Select(s => new ChapterInfo(s))
                .ToList();
        }

        public ChapterInfo GetChapter(int chapterId)
        {
            Chapter chapter = _context.Chapters.Find(chapterId);
            if(chapter == null)
            {
                return null;
            }
            return new ChapterInfo();
        }
        public ChapterInfo CreateChapter(ChapterPost chapter, string userId)
        {
            Section section = _context.Sections
                .Include(c=>c.Course)
                .Where(sec => sec.Id == chapter.SectionId)
                .FirstOrDefault();
            if(section == null || section.Course.CreatorId != userId)
            {
                return null;
            }
            Chapter newChapter = new Chapter()
            {
                Title = chapter.Title,
                Description = chapter.Description,
                Number = chapter.Number,
                SectionId = chapter.SectionId,
                VideoUrl = chapter.VideoUrl
            };
            _context.Chapters.Add(newChapter);
            _context.SaveChanges();
            return new ChapterInfo(newChapter);
        }
        public ChapterInfo EditChapter(int chapterId, ChapterPost chapter, string userId)
        {
            Chapter chapterToEdit = _context.Chapters
                .Include(ch => ch.Section)
                .ThenInclude(sec => sec.Course)
                .Where(ch => ch.Id == chapterId).FirstOrDefault();
            Section newSection = _context.Sections
                .Include(sec => sec.Course)
                .Where(sec => sec.Id == chapter.SectionId)
                .FirstOrDefault();
            
            if(chapterToEdit == null || newSection == null || chapterToEdit.Section.Course.CreatorId != userId || newSection.Course.CreatorId != userId)
            {
                return null;
            }
            chapterToEdit.Title = chapter.Title;
            chapterToEdit.Description = chapter.Description;
            chapterToEdit.Number = chapter.Number;
            chapterToEdit.SectionId = chapter.SectionId;
            chapterToEdit.VideoUrl = chapter.VideoUrl;
            _context.SaveChanges();
            return new ChapterInfo(chapterToEdit);
        }
        public ChapterInfo DeleteChapter(int chapterId, string userId)
        {
            Chapter chapterToDelete = _context.Chapters
                .Include(ch => ch.Section)
                .ThenInclude(sec => sec.Course)
                .Where(ch => ch.Id == chapterId).FirstOrDefault();
            if(chapterToDelete == null || (userId != "Moderator" && chapterToDelete.Section.Course.CreatorId != userId))
            {
                return null;
            }
            _context.Chapters.Remove(chapterToDelete);
            return new ChapterInfo(chapterToDelete);
        }
        
    }
}
