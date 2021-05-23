using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Core.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(20)]
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<RoleApplication> RoleApplications { get; set; }
    }
}
