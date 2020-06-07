using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace Plark_MobileClient.Services
{
    public class Service
    {
        protected readonly static Uri BASE_ADDRESS = new Uri("https://192.168.43.56:5001");
        protected readonly static string COOKIE_NAME = "PlarkToken";
        protected static User CurrentUser;
        protected static string TokenString;
        protected IHeadersService HeadersService = DependencyService.Get<IHeadersService>();
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
            HttpClient = new HttpClient(HttpClientHandler) { BaseAddress = BASE_ADDRESS };
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}
