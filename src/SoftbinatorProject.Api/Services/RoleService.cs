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
    public interface IRoleService
    {
        public List<RoleInfo> GetAllRoles();
        public List<RoleInfo> GetUserRoles(string id);
        public List<UserInfo> GetUsersInRole(string roleName);
        public Role CreateRole(RolePost role);
        public RoleInfo DeleteRole(int id);
        public RoleInfo AssignToRole(string userId, int roleId);
    }

    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }


        public List<RoleInfo> GetAllRoles()
        {
            return _context.Roles
                .Select(role => new RoleInfo(role))
                .ToList();
        }

        public List<RoleInfo> GetUserRoles(string id)
        {
            if (_context.Users.Find(id) == null)
            {
                return null;
            }
            return _context.Users
                .Include(u => u.Roles)
                .Where(us => us.Id == id)
                .First().Roles
                .Select(role => new RoleInfo(role))
                .ToList();
        }

        public List<UserInfo> GetUsersInRole(string roleName)
        {
            if (!_context.Roles.Where(r => r.Name.Equals(roleName)).Any())
            {
                return null;
            }
            return _context.Users
                .Include(u => u.Roles)
                .Where(user => user.Roles
                    .Select(r => r.Name)
                    .Contains(roleName))
                .Select(user => new UserInfo(user))
                .ToList();
        }

        public Role CreateRole(RolePost role)
        {
            if (_context.Roles.Where(r => r.Name.Equals(role.Name)).Any())
            {
                return null;
            }
            Role newRole = new Role { Name = role.Name };
            _context.Roles.Add(newRole);
            _context.SaveChanges();
            return newRole;
        }

        public RoleInfo DeleteRole(int id)
        {
            if (!_context.Roles.Where(r => r.Id == id).Any())
            {
                return null;
            }
            Role roleToDelete = _context.Roles.Find(id);
            _context.Roles.Remove(roleToDelete);
            _context.SaveChanges();
            return new RoleInfo(roleToDelete);
        }


        public RoleInfo AssignToRole(string userId, int roleId)
        {
            if (_context.Roles.Where(r => r.Id == (roleId)).Any() || _context.Users.Find(userId) == null)
            {
                return null;
            }
            User user = _context.Users.Include(u => u.Roles).Where(u => u.Id == userId).First();
            Role role = _context.Roles.Where(r => r.Id == (roleId)).First();
            user.Roles.Add(role);
            _context.SaveChanges();
            return new RoleInfo(role);
        }


    }
}
