using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class RolePost
    {
        [Required, MinLength(2), MaxLength(20)]
        public string Name { get; set; }
    }
}
