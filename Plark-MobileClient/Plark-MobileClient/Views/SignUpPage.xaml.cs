using Plark_MobileClient.Services;
using Plark_MobileClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                await DisplayAlert("Sign up failed.", $"Sorry something went wrong ! :(\n\nError: {responseContentAsString} ", "Ok");
                if (responseContentAsString.Contains("Email")) Email.PlaceholderColor = Color.Red;
                if (responseContentAsString.Contains("Phone")) PhoneNumber.PlaceholderColor = Color.Red;
            });
        }

        private void SignUp_Clicked(object sender, EventArgs e)
        {
            var emailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            var phoneNumberPattern = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";
            var namePattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            int error = 0;

            if (!Regex.IsMatch(Email.Text, emailPattern))
            {
                Email.PlaceholderColor = Color.Red;
                error++;
            }
            else { Email.PlaceholderColor = Color.Gray; }
            if (!Regex.IsMatch(PhoneNumber.Text, phoneNumberPattern))
            {
                PhoneNumber.PlaceholderColor = Color.Red;
                error++;
            }
            else { PhoneNumber.PlaceholderColor = Color.Gray; }
            if (!Regex.IsMatch(FirstName.Text, namePattern))
            {
                FirstName.PlaceholderColor = Color.Red;
                error++;
            }
            else { FirstName.PlaceholderColor = Color.Gray; }
            if (!Regex.IsMatch(LastName.Text, namePattern))
            {
                LastName.PlaceholderColor = Color.Red;
                error++;
            }
            else { LastName.PlaceholderColor = Color.Gray; }
            if (Password.Text != PasswordAgain.Text)
            {
                Password.PlaceholderColor = Color.Red;
                PasswordAgain.PlaceholderColor = Color.Red;
                error++;
            }
            else { Password.PlaceholderColor = Color.Gray; PasswordAgain.PlaceholderColor = Color.Gray; }

            if (error == 0) { _viewModel.SignUp(); }

        }
    }
}