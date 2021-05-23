using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class CompleteCourseInfo : CourseInfo
    {
        public virtual UserInfo Creator { get; set; }
        public virtual List<string> Users { get; set; }

        public virtual ICollection<SectionInfo> Sections { get; set; }

        public CompleteCourseInfo() { }

        public CompleteCourseInfo(Course course) : base(course)
        {
            Creator = new UserInfo(course.Creator);
            Users = course.Users
                .Select(u => u.Username)
                .ToList();


            Sections = course.Sections
                .Select(s => new SectionInfo(s))
                .ToList();
        }
    }
}
