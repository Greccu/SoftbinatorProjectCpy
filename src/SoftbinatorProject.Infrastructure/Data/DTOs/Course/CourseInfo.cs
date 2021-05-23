using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class CourseInfo : CoursePost
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatorId { get; set; }
        public virtual ICollection<string> Tags { get; set; }
        public CourseInfo() { }
        public CourseInfo(Course course) : base(course)
        {
            Id = course.Id;
            CreatedAt = course.CreatedAt;
            CreatorId = course.CreatorId;

            if(course.Tags != null)
            {
                Tags = course.Tags
                    .Select(t => t.Name)
                    .ToList();
            }
            
        }
    }
}
