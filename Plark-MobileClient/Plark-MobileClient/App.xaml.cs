using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plark_MobileClient.Services;
using Plark_MobileClient.Views;
using System.Net.Http;

namespace Plark_MobileClient
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<TicketService>();
            DependencyService.Register<UsersService>();
            DependencyService.Register<HeadersService>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
