using Plark_MobileClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plark_MobileClient.Interface
{
    public interface ITicketService
    {
        Task<ImageSource> GetTicket(TicketOption ticketOption);
        Task<string> CloseTicket();
        Task<string> GetCreationTime();
        Task<string> GetClosedTime();
        Task<ImageSource> HasActiveTicket();
        Task<bool> ArchiveUserTicket();
        Task<bool> DeleteTicket(long ticketId);
        Task<ObservableCollection<ArchivedTicket>> GetPreviousUserTickets();
    }
}
