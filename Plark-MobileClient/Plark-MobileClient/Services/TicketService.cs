using Newtonsoft.Json;
using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plark_MobileClient.Services
{
    public class TicketService : Service, ITicketService
    {
        private readonly static string GET_TICKET_URL = "/Tickets/";
        private readonly static string CLOSE_TICKET_URL = "/Tickets/CloseTicket/";
        private readonly static string GET_CREATION_TIME_URL = "/Tickets/TicketCreated/";
        private readonly static string GET_CLOSED_TIME_URL = "/Tickets/TicketClosed/";
        public TicketService() : base() { }
        public async Task<string> GetCreationTime()
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(GET_CREATION_TIME_URL);

            if (responseMessage.IsSuccessStatusCode)
            {
                return (await responseMessage.Content.ReadAsStringAsync()).Replace("\"", string.Empty);
            }
            return default;
        }

        public async Task<string> GetClosedTime()
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(GET_CLOSED_TIME_URL);

            if (responseMessage.IsSuccessStatusCode)
            {
                return (await responseMessage.Content.ReadAsStringAsync()).Replace("\"", string.Empty);
            }
            return default;
        }

        public async Task<ImageSource> HasActiveTicket()
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(GET_TICKET_URL + "ActiveTicket");
            byte[] result;

            if (responseMessage.IsSuccessStatusCode)
            {
                result = await responseMessage.Content.ReadAsByteArrayAsync();
                return ImageSource.FromStream(() => new MemoryStream(result));
            }
            return default;
        }
        public async Task<ImageSource> GetTicket(TicketOption ticketOption)
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(GET_TICKET_URL + String.Format("{0}/{1}", ticketOption.Car.Id, ticketOption.TimeInterval.Interval.TotalHours));
            byte[] result;

            if (responseMessage.IsSuccessStatusCode)
            {
                result = await responseMessage.Content.ReadAsByteArrayAsync();
                return ImageSource.FromStream(() => new MemoryStream(result));
            }
            return default;
        }

        public async Task<ObservableCollection<ArchivedTicket>> GetPreviousUserTickets()
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(GET_TICKET_URL + "ArchivedTickets");

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<ArchivedTicket>>(result);
            }
            return default;
        }

        public async Task<string> CloseTicket()
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(CLOSE_TICKET_URL + CurrentUser.UserId);

            if (responseMessage.IsSuccessStatusCode)
            {
                return (await responseMessage.Content.ReadAsStringAsync()).Replace("\"", string.Empty);
            }
            return default;
        }

        public async Task<bool> DeleteTicket(long ticketId)
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.DeleteAsync(GET_TICKET_URL + ticketId);

            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return default;
        }

        public async Task<bool> ArchiveUserTicket()
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(GET_TICKET_URL + "Archive");

            if (responseMessage.IsSuccessStatusCode)
            {
                return true

                    ;
            }
            return default;
        }
    }
}
