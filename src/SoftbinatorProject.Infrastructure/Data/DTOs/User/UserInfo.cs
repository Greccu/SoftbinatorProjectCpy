using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class UserInfo : UserPost
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public UserInfo() { }
        public UserInfo(UserPost user, string id, string email)
        {
            Id = id;
            Email = email;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfilePicture = user.ProfilePicture;

        }

        public UserInfo(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfilePicture = user.ProfilePicture;
        }
       
    }
}
