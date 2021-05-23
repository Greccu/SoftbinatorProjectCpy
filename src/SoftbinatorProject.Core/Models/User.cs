using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Core.Models
{
    public class User
    {
        public string Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string Username { get; set; }
        public string FirstName { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }
        [Url]
        public string ProfilePicture { get; set; }


        // Navigation Properties
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        [InverseProperty("Creator")]
        public virtual ICollection<Course> CreatedCoures { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<RoleApplication> RoleApplications { get; set; }
    }
}
