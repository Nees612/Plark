using Plark_MobileClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plark_MobileClient.Views
{
    [DesignTimeVisible(false)]
    public partial class TicketPage : ContentPage
    {
        TicketViewModel _ticketViewModel;
        public TicketPage()
        {
            InitializeComponent();

            BindingContext = _ticketViewModel = new TicketViewModel();
        }

        async void GetTicket_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "GetTicket");
        }
    }
}