using System;
using System.Collections.Generic;
using System.Text;

namespace Plark_Warden
{
    public class EnvironmentVariables
    {
        public static Uri BASE_ADDRESS = new Uri("https://192.168.43.56:5001");
        public static string HubUrl = "https://192.168.43.56:5001/hubs";
        public static string COOKIE_NAME = "PlarkToken";
        public static string ACTIVATE_TICKET_URL = "/Tickets/ActivateTicket/";
    }
}
