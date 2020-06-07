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
                GetTicket(null, null);

            });
            MessagingCenter.Subscribe<TicketViewModel>(this, "ShowQr", async (obj) =>
            {
                var modalPage = new QrCodePage(_viewModel.ImageSource);
                await Navigation.PushAsync(modalPage, true);
            });

        }
        protected override async void OnAppearing()
        {
            await _viewModel.SetPageData();
            var result = await _viewModel.HasActiveTicket();
            await LoadTicketButton();
            if (result)
            {
                if (_viewModel.Closed == null)
                {
                    await LoadCloseButton();
                }
                else
                {
                    await LoadPayButton();
                }
            }
            base.OnAppearing();
        }
        private async void GetTicket_Clicked(object sender, EventArgs e)
        {
            var modalPage = new TicketOptionsPage();
            await Navigation.PushAsync(modalPage, true);
        }

        private async void GetTicket(object sender, EventArgs e)
        {
            var result = await _viewModel.GetTicket();
            if (result)
            {
                if (_viewModel.Closed == null)
                {
                    await LoadCloseButton();
                }
                else
                {
                    _viewModel.CalculatePrice();
                    await LoadPayButton();
                }
            }
            else
            {
                await DisplayAlert("Ticket failed", "Failed to get ticket.", "Ok");
            }
        }

        //private async void CloseTicket_Clicked(object sender, EventArgs e)
        //{
        //    var result = await _viewModel.CloseTicket();
        //    if (result)
        //    {
        //        await DisplayAlert("Ticket Closed", "Your ticket has been closed.", "Ok");
        //        await LoadPayButton();
        //    }
        //}

        private async void Pay_Clicked(object sender, EventArgs e)
        {
            await _viewModel.ArchiveTicket();
            await _viewModel.SetPageData();
            _viewModel.ReSetData();
            await LoadTicketButton();
        }

        private Task<bool> LoadTicketButton()
        {
            var PayTicketb = (Button)FindByName("payTicketButton");
            //var CloseTicketb = (Button)FindByName("closeTicketButton");
            var GetTicketb = (Button)FindByName("getTicketButton");
            GetTicketb.IsVisible = true;
            //CloseTicketb.IsVisible = false;
            PayTicketb.IsVisible = false;
            return Task.FromResult(true);
        }

        private Task<bool> LoadCloseButton()
        {
            var PayTicketb = (Button)FindByName("payTicketButton");
            //var CloseTicketb = (Button)FindByName("closeTicketButton");
            var GetTicketb = (Button)FindByName("getTicketButton");
            GetTicketb.IsVisible = false;
            //CloseTicketb.IsVisible = true;
            PayTicketb.IsVisible = false;
            return Task.FromResult(true);
        }

        private Task<bool> LoadPayButton()
        {
            var PayTicketb = (Button)FindByName("payTicketButton");
            //var CloseTicketb = (Button)FindByName("closeTicketButton");
            var GetTicketb = (Button)FindByName("getTicketButton");
            GetTicketb.IsVisible = false;
            //CloseTicketb.IsVisible = false;
            PayTicketb.IsVisible = true;
            return Task.FromResult(true);
        }

        private async void SelectedArhivedTicket_Tapped(object sender, ItemTappedEventArgs e)
        {
            string action = await DisplayActionSheet(String.Format("Selected Ticket : {0}", _viewModel.SelectedTicket.Id), "Cancel", null, "View", "Delete");

            if (action == "View")
            {
                var modalPage = new ArchivedTicketDetailsPage(_viewModel.SelectedTicket);
                await Navigation.PushAsync(modalPage, true);
            }
            else if (action == "Delete")
            {
                var answer = await DisplayAlert(String.Format("Deleting Ticket : {0}", _viewModel.SelectedTicket.Id), "Are you sure ?", "Yes", "No");
                if (answer)
                {
                    var result = await _viewModel.DeleteSelectedTicket();
                    if (result)
                    {
                        await DisplayAlert("Deleting Car", "Succesfully deleted your ticket !", "Ok");
                        await _viewModel.SetPageData();
                    }
                    else
                    {
                        await DisplayAlert(String.Format("Selected Ticket : {0}", _viewModel.SelectedTicket.Id), "Failed to delete ticket !", "Ok");
                    }
                }
            }


        }
    }
}