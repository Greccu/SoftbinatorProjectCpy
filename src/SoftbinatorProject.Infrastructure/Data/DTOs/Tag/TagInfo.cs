using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class TagInfo : TagPost
    {
        public int Id { get; set; }
        public TagInfo() { }
        public TagInfo(Tag tag) : base(tag)
        {
            Id = tag.Id;
        }
    }
}
