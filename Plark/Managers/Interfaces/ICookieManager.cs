using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Managers.Interfaces
{
    public interface ICookieManager
    {
        public CookieOptions CreateCookieOption(int? expireTime = 1400);
    }
}
