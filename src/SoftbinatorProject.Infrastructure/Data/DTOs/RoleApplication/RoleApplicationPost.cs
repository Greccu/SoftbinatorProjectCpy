using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class RoleApplicationPost
    {
        public int RoleId { get; set; }
        [Required, MinLength(2)]
        public string Content { get; set; }

        public RoleApplicationPost() { }

        public RoleApplicationPost(RoleApplication roleApplication)
        {
            RoleId = roleApplication.RoleId;
            Content = roleApplication.Content;
        }
    }
}
