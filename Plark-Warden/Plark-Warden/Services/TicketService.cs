using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plark_Warden.Services
{
    public class TicketService : Service, ITicketService
    {
        public TicketService() : base() { }
        public async Task<bool> ActivateTicket(long ticketId)
        {
            //HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(EnvironmentVariables.ACTIVATE_TICKET_URL + ticketId);

            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return default;
        }
    }
}
