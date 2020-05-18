using Plark_MobileClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Xamarin.Forms;
using Plark_MobileClient.Views;

namespace Plark_MobileClient.ViewModels
{
    public class TicketViewModel : BaseViewModel
    {
        private readonly static string REQUEST_URL = "/Tickets/7";
        public Ticket ticket;

        public Ticket Ticket
        {
            get { return ticket; }
            set
            {
                SetProperty(ref ticket, value);
            }
        }

        private HttpClient HttpClient => DependencyService.Get<HttpClient>();

        public TicketViewModel()
        {
            Ticket = new Ticket();
            Ticket.ByteString = "asd";
            HttpClient.BaseAddress = new Uri("https://192.168.100.12:5001");
            MessagingCenter.Subscribe<TicketPage>(this, "GetTicket", async (obj) =>
            {
                var responseMessage = await HttpClient.GetAsync(REQUEST_URL);
                var result = await responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine(result);
                Ticket = new Ticket() { ByteString = result };
            });
        }

    }
}
