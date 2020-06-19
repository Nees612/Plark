using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Plark_Warden.Services
{
    public class HeadersService : IHeadersService
    {
        public Task<AuthenticationHeaderValue> GetAuthenticationHeader(string authJwtToken)
        {
            return Task.FromResult(new AuthenticationHeaderValue("Bearer", authJwtToken));
        }
    }
}
