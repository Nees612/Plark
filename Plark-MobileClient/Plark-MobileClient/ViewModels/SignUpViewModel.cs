using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using Xamarin.Forms;

namespace Plark_MobileClient.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private string firstName;
        private string lastName;
        private string email;
        private string phoneNumber;
        private string password;
        private string passwordAgain;

        public string FirstName { get => firstName; set { SetProperty(ref firstName, value); } }
        public string LastName { get => lastName; set { SetProperty(ref lastName, value); } }
        public string Email { get => email; set { SetProperty(ref email, value); } }
        public string PhoneNumber { get => phoneNumber; set { SetProperty(ref phoneNumber, value); } }
        public string Password { get => password; set { SetProperty(ref password, value); } }
        public string PasswordAgain { get => passwordAgain; set { SetProperty(ref passwordAgain, value); } }

        private IUsersService _usersService => DependencyService.Get<IUsersService>();

        public async void SignUp()
        {
            var newUser = new NewUser() { FirstName = firstName, LastName = lastName, EmailAddress = email, PhoneNumber = phoneNumber, Password = Password, PasswordAgain = PasswordAgain };
            await _usersService.SignUp(newUser);
        }
    }
}
