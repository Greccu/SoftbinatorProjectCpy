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
    public interface ITagService
    {
        public TagInfo GetTag(int tagId);
        public List<TagInfo> GetAllTags();
        public TagInfo CreateTag(TagPost tag);
        public TagInfo AssignTag(int courseId, int tagId, string userId);
        public TagInfo UpdateTag(int tagId, TagPost tag);
        public TagInfo DeleteTag(int tagId);

    }
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public TagInfo GetTag(int tagId)
        {
            Tag tag = _context.Tags.Find(tagId);
            if(tag == null)
            {
                return null;
            }
            return new TagInfo(tag);
        }
        public List<TagInfo> GetAllTags()
        {
            return _context.Tags.Select(tg => new TagInfo(tg)).ToList();
        }
        public TagInfo CreateTag(TagPost tag)
        {
            Tag tagToAdd = new Tag
            {
                Name = tag.Name
            };
            _context.Tags.Add(tagToAdd);
            _context.SaveChanges();
            return new TagInfo(tagToAdd);
        }
        public TagInfo AssignTag(int courseId, int tagId, string userId)
        {
            Course course = _context.Courses
                .Include(c => c.Tags)
                .Where(c => c.Id == courseId)
                .FirstOrDefault();
            Tag tag = _context.Tags.Find(tagId);
            if (course == null || tag == null || course.CreatorId != userId)
            {
                return null;
            }
            course.Tags.Add(tag);
            _context.SaveChanges();
            return new TagInfo(tag);
        }
        public TagInfo UpdateTag(int tagId, TagPost tag)
        {
            Tag tagToUpdate = _context.Tags.Find(tagId);
            if(tagToUpdate == null)
            {
                return null;
            }
            tagToUpdate.Name = tag.Name;
            _context.SaveChanges();
            return new TagInfo(tagToUpdate);
        }
        public TagInfo DeleteTag(int tagId) {
            Tag tagToRemove = _context.Tags.Find(tagId);
            if(tagToRemove == null)
            {
                return null;
            }
            _context.Tags.Remove(tagToRemove);
            _context.SaveChanges();
            return new TagInfo(tagToRemove);
        }
    }
}
