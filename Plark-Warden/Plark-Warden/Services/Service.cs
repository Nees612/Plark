using Plark_Warden.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace Plark_Warden.Services
{
    public class Service
    {
        protected static Warden CurrentWarden;
        protected static string TokenString;
        //protected IHeadersService HeadersService => DependencyService.Get<IHeadersService>();
        protected HttpClientHandler HttpClientHandler;
        protected CookieContainer CookieContainer;
        protected static HttpClient HttpClient;
        public Service()
        {
            CookieContainer = new CookieContainer();
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    HttpClientHandler = DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler(ref CookieContainer);
                    break;
                default:
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    HttpClientHandler = new HttpClientHandler() { CookieContainer = this.CookieContainer };
                    break;
            }
            HttpClient = new HttpClient(HttpClientHandler) { BaseAddress = EnvironmentVariables.BASE_ADDRESS };
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
