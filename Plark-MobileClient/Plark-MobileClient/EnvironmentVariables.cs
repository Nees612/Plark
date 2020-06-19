using System;
using System.Collections.Generic;
using System.Text;

namespace Plark_MobileClient
{
    public static class EnvironmentVariables
    {
        public static Uri BASE_ADDRESS = new Uri("https://192.168.43.56:5001");
        public static string HubUrl = "http://192.168.43.56:5000/hubs/tickets";
        public static string COOKIE_NAME = "PlarkToken";
    }
}
