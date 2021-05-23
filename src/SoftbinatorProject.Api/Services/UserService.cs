using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftbinatorProject.Core.Constants;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Services
{
    public interface IUserService
    {
        public UserInfo GetUserInfo(string id);
        public UserInfo CreateUserProfile(UserInfo user);
        public List<UserInfo> GetAllUsers();
        public UserInfo EditUserProfile(string id, UserPost user);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;




        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public UserInfo GetUserInfo(string id)
        {
            User user = _context.Users.Find(id); 
            if(user == null)
            {
                return null;
            }
            return new UserInfo(user);
        }

        public UserInfo CreateUserProfile(UserInfo user)
        {
            User newUser = new User
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = user.ProfilePicture != "" ? user.ProfilePicture : UserConstants.DEFAULT_PROFILE_PICTURE 
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return new UserInfo(newUser);
        }

        public List<UserInfo> GetAllUsers()
        {
            return _context.Users.Select(user => new UserInfo(user)).ToList();
        }

        public UserInfo EditUserProfile(string id, UserPost user)
        {
            User userToEdit = _context.Users.Find(id);
            if(userToEdit == null)
            {
                return null;
            }
            userToEdit.FirstName = user.FirstName;
            userToEdit.LastName = user.LastName;
            userToEdit.Username = user.Username;
            userToEdit.ProfilePicture = user.ProfilePicture;
            _context.SaveChanges();
            return new UserInfo(userToEdit);
        }

    }
}
