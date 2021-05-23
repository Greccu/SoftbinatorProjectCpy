using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class TagPost
    {
        [Required, MinLength(2), MaxLength(30)]
        public string Name { get; set; }
        public TagPost() { }
        public TagPost(Tag tag)
        {
            Name = tag.Name;
        }
    }
}
