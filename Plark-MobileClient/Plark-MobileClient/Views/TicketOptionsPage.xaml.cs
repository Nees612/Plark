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
    public partial class TicketOptionsPage : ContentPage
    {
        private readonly TicketOptionsViewModel _viewModel;
        public TicketOptionsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TicketOptionsViewModel();
        }

        protected override async void OnAppearing()
        {
            await _viewModel.SetPageData();
            base.OnAppearing();
        }

        private async void ReceiveTicket_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Receive Ticket", "Are you sure ?", "Yes", "No");
            if (result)
            {
                MessagingCenter.Send(this, "TicketOptions", new TicketOption { TimeInterval = _viewModel.SelectedInterval, Car = _viewModel.SelectedCar });
                await Navigation.PopAsync(true);
            }

        }
    }
}