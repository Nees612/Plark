using Java.Lang;
using Plark_MobileClient.Models;
using Plark_MobileClient.ViewModels;
using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plark_MobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel _viewModel;
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ProfileViewModel();
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            _viewModel.Logout();
        }

        protected override async void OnAppearing()
        {
            await _viewModel.SetViewData();
            base.OnAppearing();
        }

        private async void AddCar_Clicked(object sender, EventArgs e)
        {
            Regex RX = new Regex(@"^[a-zA-Z]{3}[- ]?[0-9]{3}|[mM][- ]?[0-9]{6}|[cC][kK][- ]?[0-9]{2}[- ]?
                                [0-9]{2}|[dD][tT][- ]?[0-9]{2}[- ]?[0-9]{2}|[hH][cC][- ]?[0-9]{2}[- ]?[0-9]{2}|[cC][dD][- ]?[0-9]{2}[- ]?
                                [0-9]{2}|[cC][dD][- ]?[0-9]{3}[- ]?[0-9]{3}|[hH][aAbBeEfFiIkKlLmMnNpPrRsStTvVxX][- ]?[0-9]{2}[- ]?[0-9]{2}|
                                [mM][aA][- ]?[0-9]{2}[- ]?[0-9]{2}|[oO][tT][- ]?[0-9]{2}[- ]?[0-9]{2}|[rR][a-zA-Z][- ]?[0-9]{2}[- ]?[0-9]{2}|
                                [rR]{2}[- ]?[0459][0-9][- ]?[0-9]{2}|[cC][- ]?[cCxX][- ]?[0-9]{2}[- ]?[0-9]{2}|[xX][- ]?[aAbBcC][- ]?[0-9]{2}[- ]?
                                [0-9]{2}|[eEpPvVzZ][- ]?[0-9]{5}|[sS][- ]?[a-zA-z]{3}[- ]?[0-9]{3}|[sS][pP][- ]?[0-9]{2}[- ]?[0-9]{2}|[a-zA-Z]{4}[- ]?
                                [0-9][- ]?[0-9]|[a-zA-Z]{5}[- ]?[0-9]$");

            string NumberPlate = await DisplayPromptAsync("New Numberplate", "Enter your numberplate!");
            if (NumberPlate != null && RX.IsMatch(NumberPlate.ToUpper()))
            {
                var Result = await _viewModel.AddNewCar(NumberPlate.ToUpper());
                if (!Result)
                {
                    await DisplayAlert("Numberplate", "This numberplate is already exists.", "Ok");
                }
            }
            else if (NumberPlate != null)
            {
                await DisplayAlert("Numberplate", "This is an invalid numberplate", "Ok");
            }
        }

        private async void CarList_CarTapped(object sender, ItemTappedEventArgs e)
        {
            string action = await DisplayActionSheet($"Car : {_viewModel.SelectedCar.NumberPlate}", "Cancel", null, "Edit", "Delete");

            if (action == "Delete")
            {
                var answer = await DisplayAlert($"Deleting : {_viewModel.SelectedCar.NickName}", "Are you sure ?", "Yes", "No");
                if (answer)
                {
                    var result = await _viewModel.DeleteCar();
                    if (result)
                    {
                        await DisplayAlert("Deleting Car", "Succesfully deleted your car !", "Ok");
                        await _viewModel.SetViewData();
                    }
                    else
                    {
                        await DisplayAlert("Deleting Car", "Failed to delete your car !", "Ok");
                    }
                }
            }
            else if (action == "Edit")
            {
                string editAction = await DisplayActionSheet("Car : " + _viewModel.SelectedCar.NumberPlate, "Cancel", null, "Nickname", "Numberplate");
                if (editAction == "Nickname")
                {
                    string nickname = await DisplayPromptAsync("NickName", "Enter Nickname.");
                    if (nickname != null || nickname != string.Empty)
                    {
                        var result = await _viewModel.UpdateSelectedCar(new Car
                        {
                            Id = _viewModel.SelectedCar.Id,
                            NickName = nickname,
                            NumberPlate = _viewModel.SelectedCar.NumberPlate
                        });
                        if (!result) await DisplayAlert($"Editing Car: {_viewModel.SelectedCar.NickName}", "Failed to rename your car.", "Ok");
                    }
                }
                else if (editAction == "Numberplate")
                {
                    string numberplate = await DisplayPromptAsync("Numberplate", "Enter Numberplate.");
                    if (numberplate != null || numberplate != string.Empty)
                    {
                        var result = await _viewModel.UpdateSelectedCar(new Car
                        {
                            Id = _viewModel.SelectedCar.Id,
                            NickName = _viewModel.SelectedCar.NickName,
                            NumberPlate = numberplate.ToUpper()
                        });
                        if (!result) await DisplayAlert($"Editing Car: {_viewModel.SelectedCar.NickName}", "Failed to change numberplate.", "Ok");
                    }
                }
            }


        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}