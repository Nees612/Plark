using Plark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Plark.ViewModels;

namespace Plark.Managers.Interfaces
{
    public interface IUserManager
    {
        public User CreateUser(UserViewModel model);
        public string CreateHash(string password);
        public bool IsValidPasssword(string correctHash, string Password);
        public string GenerateLoginJwtToken(User user, int? expireTime = 1400);
        public string GenerateEmailVerificationToken(User user, int? expireTime = 1400);
        public long GetUserIdFromToken(string header);
        public bool IsEmailTokenValid(string emailToken);
    }
}
