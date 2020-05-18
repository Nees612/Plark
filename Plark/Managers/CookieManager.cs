
using Microsoft.AspNetCore.Http;
using Plark.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Managers
{
    public class CookieManager : ICookieManager
    {
        public CookieOptions CreateCookieOption(int? expireTime = 1400)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            return option;
        }
    }
}
