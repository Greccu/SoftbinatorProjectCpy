using Microsoft.EntityFrameworkCore;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Services
{
    public interface IRoleApplicationService
    {
        public List<RoleApplicationInfo> GetRoleApplications(string status);
        public RoleApplicationInfo GetRoleApplication(int id);
        public RoleApplicationInfo ChangeStatus(int id, string status);
        public RoleApplicationInfo CreateRoleApplication(string userId, RoleApplicationPost roleApplication);
        public RoleApplicationInfo DeleteRoleApplication(int id);
    }
    public class RoleApplicationService : IRoleApplicationService
    {
        private readonly AppDbContext _context;
        private readonly IRoleService _roleService;
        public RoleApplicationService(AppDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        public List<RoleApplicationInfo> GetRoleApplications(string status)
        {
            return _context.RoleApplications.Where(ra => status == "*" || ra.Status == status).Select(ra => new RoleApplicationInfo(ra)).ToList();
        }
        public RoleApplicationInfo GetRoleApplication(int id)
        {
            RoleApplication ra = _context.RoleApplications.Find(id);
            if(ra == null)
            {
                return null;
            }
            return new RoleApplicationInfo(ra);
        }
        public RoleApplicationInfo ChangeStatus(int id, string status)
        {
            RoleApplication ra = _context.RoleApplications.Find(id);
            if (ra == null)
            {
                return null;
            }
            try
            {
                ra.Status = status;
                _context.SaveChanges();
                if (status.Equals("Accepted"))
                {
                    var ret = _roleService.AssignToRole(ra.UserId, ra.RoleId);
                    if(ret == null)
                    {
                        return null;
                    }

                }
                return new RoleApplicationInfo(ra);
            }
            catch(Exception)
            {
                return null;
            }
            
        }
        public RoleApplicationInfo CreateRoleApplication(string userId, RoleApplicationPost roleApplication)
        {
            if(_context.Roles.Find(roleApplication.RoleId) == null || _context.Users.Find(userId) == null)
            {
                return null;
            }
            RoleApplication ra = new RoleApplication
            {
                RoleId = roleApplication.RoleId,
                Content = roleApplication.Content,
                Status = "Created"
            };
            _context.RoleApplications.Add(ra);
            _context.SaveChanges();
            return new RoleApplicationInfo(ra);
        }

        public RoleApplicationInfo DeleteRoleApplication(int id)
        {
            RoleApplication ra = _context.RoleApplications.Find(id);
            if (ra == null || ra.Status != "Created")
            {
                return null;
            }
            _context.RoleApplications.Remove(ra);
            _context.SaveChanges();
            return new RoleApplicationInfo(ra);
        }
    }
}
