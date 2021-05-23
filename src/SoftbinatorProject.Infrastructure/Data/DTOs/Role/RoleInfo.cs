using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class RoleInfo:RolePost
    {
        public int Id { get; set; }

        public RoleInfo(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }
    }
}
