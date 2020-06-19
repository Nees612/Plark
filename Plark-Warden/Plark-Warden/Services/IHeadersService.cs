using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Plark_Warden.Services
{
    public interface IHeadersService
    {
        Task<AuthenticationHeaderValue> GetAuthenticationHeader(string authJwtToken);
    }
}
