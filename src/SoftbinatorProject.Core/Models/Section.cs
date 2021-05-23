using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Core.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public int? Number { get; set; }
        [Required, MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int CourseId { get; set; }

        // Navigation Properties
        public virtual Course Course { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
    }
}
