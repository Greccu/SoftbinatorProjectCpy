using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class SectionPost
    {
        public int? Number { get; set; }
        [Required, MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int CourseId { get; set; }

        public SectionPost() { }
        public SectionPost(Section section)
        {
            Title = section.Title;
            Description = section.Description;
            CourseId = section.CourseId;
            Number = section.Number;
        }
    }
}
