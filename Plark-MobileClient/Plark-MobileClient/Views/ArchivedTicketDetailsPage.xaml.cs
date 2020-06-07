using Plark_MobileClient.Models;
using Plark_MobileClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plark_MobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArchivedTicketDetailsPage : ContentPage
    {
        private ArchivedTicket _ticket;
        private ArchivedTicketViewModel _viewModel;
        public ArchivedTicketDetailsPage(ArchivedTicket ticket)
        {
            InitializeComponent();
            _ticket = ticket;
            BindingContext = _viewModel = new ArchivedTicketViewModel();
        }

        protected override void OnAppearing()
        {
            _viewModel.SetPageData(_ticket);
            base.OnAppearing();
        }
    }
}