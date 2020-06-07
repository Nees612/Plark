using Plark_MobileClient.Services;
using Plark_MobileClient.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Plark_MobileClient.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        private MainPageViewModel _viewModel;

        private NavigationPage _ticketPage;
        private NavigationPage _profilePage;
        private NavigationPage _loginPage;
        private NavigationPage _signUpPage;
        
        public MainPage()
        {
            InitializeComponent();

            _viewModel = new MainPageViewModel();

            _ticketPage = new NavigationPage(new TicketPage()) { Title = "Ticket" };
            _profilePage = new NavigationPage(new ProfilePage()) { Title = "Profile" };
            _loginPage = new NavigationPage(new LoginPage()) { Title = "Login" };
            _signUpPage = new NavigationPage(new SignUpPage()) { Title = "SignUp" };

            _viewModel.IsUserLoggedIn();
            MessagingCenter.Subscribe<UsersService>(this, "UserLoggedIn", (obj) =>
            {
                LoadUserView();
            });
            MessagingCenter.Subscribe<UsersService>(this, "UserIsNotLoggedIn", (obj) =>
            {
                LoadMainView();
            });

        }

        private void LoadUserView()
        {
            Children.Clear();
            Children.Add(_ticketPage);
            Children.Add(_profilePage);
        }

        private void LoadMainView()
        {
            Children.Clear();
            Children.Add(_loginPage);
            Children.Add(_signUpPage);
        }

    }
}