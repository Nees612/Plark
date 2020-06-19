using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Plark.Managers.Interfaces;
using Plark.Models;
using Plark.Models.Interface;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Plark.Managers
{
    public class TicketManager : ITicketManager
    {
        private IConfiguration _config;
        public TicketManager(IConfiguration configuration)
        {
            _config = configuration;
        }
        public Task<byte[]> BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return Task.FromResult(stream.ToArray());
            }
        }

        public Task<Bitmap> CreateQrcode(string data)
        {
            QRCodeGenerator codeGenerator = new QRCodeGenerator();
            var QrData = codeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var Qr = new QRCode(QrData);

            return Task.FromResult(Qr.GetGraphic(20));
        }

        public Task<string> GenerateTicketJwtToken(User user, Car car, double expireTime)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var created = DateTime.Now;
            var expires = created.AddHours(expireTime);

            var claims = new[] {
                new Claim("UserId", user.Id.ToString() ),
                new Claim("CarId", car.Id.ToString() ),
                new Claim("NumberPlate", car.NumberPlate),
                new Claim("Created", created.ToString()),
                new Claim("Expires", expires.ToString()),
                new Claim("Price", (expireTime * 250).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var JWToken = new JwtSecurityToken(
             issuer: "https://*:5001",
             audience: "plarkMobile",
             claims: claims,
             notBefore: new DateTimeOffset(created).DateTime,
             expires: new DateTimeOffset(expires).DateTime,
             signingCredentials: credentials
         );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);

            return Task.FromResult(token);
        }

        private JwtSecurityToken ConvertToken(string emailToken)
        {
            var token = emailToken.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS;
        }
        public Task<string> GetCreatedFromToken(string token)
        {
            var tokenS = ConvertToken(token);

            return Task.FromResult(tokenS.Claims.First(c => c.Type.Equals("Created")).Value);
        }

        public Task<string> GetExpireFromToken(string token)
        {
            var tokenS = ConvertToken(token);

            return Task.FromResult(tokenS.Claims.First(c => c.Type.Equals("Expires")).Value);
        }
    }
}
