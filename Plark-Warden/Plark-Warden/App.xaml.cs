using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plark_Warden.Services;
using Plark_Warden.Views;

namespace Plark_Warden
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<ScannerService>();
            DependencyService.Register<JwtDecoderService>();
            DependencyService.Register<TicketService>();
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
