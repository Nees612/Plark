using Plark_MobileClient.Interface;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Plark_MobileClient.Services
{
    public class HeadersService : IHeadersService
    {
        public Task<AuthenticationHeaderValue> GetAuthenticationHeader(string authJwtToken)
        {
            return Task.FromResult(new AuthenticationHeaderValue("Bearer", authJwtToken));
        }
    }
}
