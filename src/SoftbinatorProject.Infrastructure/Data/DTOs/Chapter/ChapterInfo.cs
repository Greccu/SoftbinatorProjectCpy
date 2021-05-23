using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class ChapterInfo : ChapterPost
    {
        public int Id { get; set; }

        public ChapterInfo() { }
        public ChapterInfo(Chapter chapter) : base(chapter)
        {
            Id = chapter.Id;
        }
    }
}
