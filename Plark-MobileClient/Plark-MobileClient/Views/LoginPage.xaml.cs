using Plark_MobileClient.Services;
using Plark_MobileClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plark_MobileClient.Views
{
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel();

            MessagingCenter.Subscribe<UsersService>(this, "LoginFailed", async (obj) =>
            {
                await DisplayAlert("Login failed", "Email or password is invalid !", "Ok");
            });
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            _viewModel.Login();
        }
    }
}