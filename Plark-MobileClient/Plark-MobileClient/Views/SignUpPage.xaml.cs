using Plark_MobileClient.Services;
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
    public partial class SignUpPage : ContentPage
    {
        private SignUpViewModel _viewModel;
        public SignUpPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new SignUpViewModel();

            MessagingCenter.Subscribe<UsersService>(this, "VerifyEmail", async (obj) =>
            {
                await DisplayAlert("Email verification.", "We sent you an email with verification URL, please verify your email!", "Ok");

            });

            MessagingCenter.Subscribe<UsersService, string>(this, "SignUpFailed", async (obj, responseContentAsString) =>
            {
                await DisplayAlert("Sign up failed.", "Please check your input !", "Ok");
            });
        }

        private void SignUp_Clicked(object sender, EventArgs e)
        {
            _viewModel.SignUp();
        }
    }
}