using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Plark_Warden.Services
{
    public interface IHTTPClientHandlerCreationService
    {
        HttpClientHandler GetInsecureHandler(ref CookieContainer cookieContainer);
    }
}
