using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class UserPost
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Username { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }
        [Url]
        public string ProfilePicture { get; set; }
    }
}
