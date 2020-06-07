using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Plark_MobileClient.Interface
{
    public interface IHTTPClientHandlerCreationService
    {
        HttpClientHandler GetInsecureHandler(ref CookieContainer cookieContainer);
    }
}
