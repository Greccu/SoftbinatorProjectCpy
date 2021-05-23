using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class SectionInfo : SectionPost
    {
        public int Id { get; set; }
        public List<ChapterInfo> Chapters { get; set;}

        public SectionInfo() { }
        public SectionInfo(Section section) : base(section)
        {
            Id = section.Id;

            Chapters = section.Chapters
                .Select(c => new ChapterInfo(c))
                .ToList();
        }
    }
}
