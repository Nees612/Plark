using Plark_MobileClient.Interface;
using Plark_MobileClient.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plark_MobileClient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string email;
        private string password;
        private bool rememberMe = false;

        public string Email
        {
            get => email;
            set { SetProperty(ref email, value); }
        }
        public string Password
        {
            get => password;
            set { SetProperty(ref password, value); }
        }

        public bool RememberMe
        {
            get => rememberMe;
            set { SetProperty(ref rememberMe, value); }
        }

        private IUsersService _usersService => DependencyService.Get<IUsersService>();

        public async void Login()
        {
            await _usersService.Login(Email, Password, RememberMe);
            Email = Password = null;
            RememberMe = false;
        }
    }
}
