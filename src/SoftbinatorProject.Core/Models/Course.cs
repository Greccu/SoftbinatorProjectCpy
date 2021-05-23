using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Core.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatorId { get; set; }


        // Navigation Properties
        //[InverseProperty("CreatedCourses")]
        public virtual User Creator { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
    }
}
