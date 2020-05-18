using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Plark.Managers
{
    public class UserManager : IUserManager
    {
        private static readonly string HARDCODED_SALT = "^P~LL~˘AA~RK";
        private IConfiguration _config;
        public UserManager(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string CreateHash(string password)
        {
            var passwordToHash = password + HARDCODED_SALT;
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordToHash, BCrypt.Net.BCrypt.GenerateSalt());

            return passwordHash;
        }

        public bool IsValidPasssword(string correctHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password + HARDCODED_SALT, correctHash);
        }

        public User CreateUser(UserViewModel model)
        {
            return new User { FirstName = model.FirstName, LastName = model.LastName, EmailAddress = model.EmailAddress, PhoneNumber = model.PhoneNumber, IsEmailVerified = false };
        }

        public string GenerateLoginJwtToken(User user, int? expireTime = 1400)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("UserId", user.Id.ToString() ),
                new Claim("UserLastName", user.LastName ),
                new Claim("UserFirstName", user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var JWToken = new JwtSecurityToken(
             issuer: "http://localhost:4200/",
             audience: "http://localhost:4200/",
             claims: claims,
             notBefore: new DateTimeOffset(DateTime.Now).DateTime,
             expires: new DateTimeOffset(DateTime.Now.AddMinutes(expireTime.Value)).DateTime,
             signingCredentials: credentials
         );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);

            return token;
        }

        public string GenerateEmailVerificationToken(User user, int? expireTime = 1400)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("UserId", user.Id.ToString() ),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var JWToken = new JwtSecurityToken(
             issuer: "http://localhost:4200/",
             audience: "http://localhost:4200/",
             claims: claims,
             notBefore: new DateTimeOffset(DateTime.Now).DateTime,
             expires: new DateTimeOffset(DateTime.Now.AddMinutes(expireTime.Value)).DateTime,
             signingCredentials: credentials
         );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);

            return token;
        }

        private JwtSecurityToken ConvertToken(string emailToken)
        {
            var token = emailToken.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS;
        }

        public bool IsEmailTokenValid(string emailToken)
        {
            var tokenS = ConvertToken(emailToken);

            return (tokenS.ValidTo > DateTime.Now);
        }
        public long GetUserIdFromToken(string emailToken)
        {
            var tokenS = ConvertToken(emailToken);

            return long.Parse(tokenS.Claims.First(c => c.Type.Equals("UserId")).Value);
        }
    }
}
