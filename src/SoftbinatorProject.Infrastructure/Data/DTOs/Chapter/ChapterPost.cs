using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class ChapterPost
    {
        public int? Number { get; set; }
        [Required,MinLength(2),MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int SectionId { get; set; }
        [Url]
        public string VideoUrl { get; set; }

        public ChapterPost() { }
        public ChapterPost(Chapter chapter)
        {
            Title = chapter.Title;
            Description = chapter.Description;
            SectionId = chapter.SectionId;
            Number = chapter.Number;
        }
    }
}
