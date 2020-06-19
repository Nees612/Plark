using Plark_MobileClient.Models;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketPage : ContentPage
    {
        private readonly TicketViewModel _viewModel;
        public TicketPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TicketViewModel();

            MessagingCenter.Subscribe<TicketOptionsPage, TicketOption>(this, "TicketOptions", (obj, ticketOption) =>
            {
                _viewModel.TicketOption = ticketOption;
                GetTicket();

            });
            MessagingCenter.Subscribe<TicketViewModel>(this, "ShowQr", async (obj) =>
            {
                var modalPage = new QrCodePage(_viewModel.ImageSource);
                await Navigation.PushAsync(modalPage, true);
            });

        }
        protected override async void OnAppearing()
        {
            await _viewModel.SetPreviousTickets();
            var hasTicketIdQrImage = await _viewModel.HasTicketIdQrImage();

            if (hasTicketIdQrImage)
            {
                Information.IsVisible = true;
                GetTicketButton.IsVisible = false;
                PayTicketButton.IsVisible = false;
            }
            else
            {
                PayTicketButton.IsVisible = false;
                GetTicketButton.IsVisible = true;
            }

            var hasActiveTicket = await _viewModel.HasActiveTicket();

            if (hasActiveTicket)
            {
                Information.IsVisible = false;
                GetTicketButton.IsVisible = false;
                PayTicketButton.IsVisible = true;
            }

            base.OnAppearing();
        }
        private async void GetTicket_Clicked(object sender, EventArgs e)
        {
            var modalPage = new TicketOptionsPage();
            await Navigation.PushAsync(modalPage, true);
        }

        private async void GetTicket()
        {
            var result = await _viewModel.GetTicketIdQr();
            if (!result)
            {
                await DisplayAlert("Ticket failed", "Failed to get ticket.", "Ok");
            }
        }

        private async void Pay_Clicked(object sender, EventArgs e)
        {
            await _viewModel.ArchiveTicket();

            PayTicketButton.IsVisible = false;
            GetTicketButton.IsVisible = true;
        }

        private async void SelectedArhivedTicket_Tapped(object sender, ItemTappedEventArgs e)
        {
            string action = await DisplayActionSheet($"Selected Ticket : {_viewModel.SelectedTicket.Id}", "Cancel", null, "View", "Delete");

            if (action == "View")
            {
                var modalPage = new ArchivedTicketDetailsPage(_viewModel.SelectedTicket);
                await Navigation.PushAsync(modalPage, true);
            }
            else if (action == "Delete")
            {
                var answer = await DisplayAlert($"Deleting Ticket : {_viewModel.SelectedTicket.Id}", "Are you sure ?", "Yes", "No");
                if (answer)
                {
                    var deletedId = _viewModel.SelectedTicket.Id;
                    var result = await _viewModel.DeleteSelectedTicket();
                    if (result)
                    {
                        await DisplayAlert($"Deleting Ticket : {deletedId}", "Succesfully deleted your ticket !", "Ok");
                        await _viewModel.SetPreviousTickets();
                    }
                    else
                    {
                        await DisplayAlert($"Selected Ticket : {deletedId}", "Failed to delete ticket !", "Ok");
                    }
                }
            }


        }
    }
}