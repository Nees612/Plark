using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plark_Warden.Services
{
    public interface IJwtDecoderService
    {
        Task<Dictionary<string, string>> DecodeJwt(string tokenString);
        Task<bool> CanReadToken(string tokenString);
    }
}
