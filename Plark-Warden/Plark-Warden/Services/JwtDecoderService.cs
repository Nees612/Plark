using Plark_Warden.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using ZXing.Aztec.Internal;

namespace Plark_Warden.Services
{
    public class JwtDecoderService : IJwtDecoderService
    {
        private JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        public Task<Dictionary<string, string>> DecodeJwt(string tokenString)
        {
            var tokenS = handler.ReadToken(tokenString) as JwtSecurityToken;
            var Data = new Dictionary<string, string>();

            tokenS.Claims.ForEach(c =>
            {
                Data.Add(c.Type, c.Value);
            });

            return Task.FromResult(Data);

        }

        public Task<bool> CanReadToken(string tokenString)
        {
            var result = handler.CanReadToken(tokenString);

            return Task.FromResult(result);
        }
    }
}


