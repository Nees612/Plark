using Plark_MobileClient.Interface;
using Plark_MobileClient.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plark_MobileClient.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        private IUsersService _usersService => DependencyService.Get<IUsersService>();

        public async void IsUserLoggedIn()
        {
            await _usersService.IsUserLoggedIn();
        }
    }
}
