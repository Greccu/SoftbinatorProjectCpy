using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class RoleApplicationInfo : RoleApplicationPost
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }

        public RoleApplicationInfo() { }
        public RoleApplicationInfo(RoleApplication roleApplication) : base(roleApplication)
        {
            Id = roleApplication.Id;
            UserId = roleApplication.UserId;
            Status = roleApplication.Status;
        }
    }
}
