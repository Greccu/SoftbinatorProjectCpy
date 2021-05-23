using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.AuthorizationHandler
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private AppDbContext _dbContext;

        public RolesAuthorizationHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var validRole = false;
            if (requirement.AllowedRoles == null ||
                requirement.AllowedRoles.Any() == false)
            {
                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var roles = requirement.AllowedRoles;
                var user = _dbContext.Users
                    .Include(u => u.Roles)
                    .Where(us => us.Id == userId)
                    .FirstOrDefault();
                var userRoles = user.Roles;
                validRole = userRoles.Any(role => roles.Contains(role.Name));
            }

            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
