using Newtonsoft.Json;
using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plark_MobileClient.Services
{
    public class UsersService : Service, IUsersService
    {
        private readonly static string ISTOKENVALID_URL = "/Users";
        private readonly static string LOGIN_URL = "/Users/SignIn";
        private readonly static string SIGNUP_URL = "/Users/SignUp";
        private readonly static string CARS_URL = "/Cars/";

        public UsersService() : base() { }
        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            string jsonData = JsonConvert.SerializeObject(new { Email = email, Password = password });

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(LOGIN_URL, content);
            if (response.IsSuccessStatusCode)
            {
                TokenString = response.Headers.FirstOrDefault(h => h.Key.Equals("Set-Cookie")).Value.Where(v => v.Contains(COOKIE_NAME)).FirstOrDefault().Split(';').First().Replace(COOKIE_NAME + "=", string.Empty);
                HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
                SetCurrentUser(TokenString);
                if (rememberMe) Preferences.Set(COOKIE_NAME, TokenString);
                MessagingCenter.Send(this, "UserLoggedIn");
                return true;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                MessagingCenter.Send(this, "LoginFailed");
                return false;
            }
        }


        public async Task<bool> IsUserLoggedIn()
        {
            var tokenString = Preferences.Get(COOKIE_NAME, string.Empty);
            if (tokenString.Equals(string.Empty))
            {
                MessagingCenter.Send(this, "UserIsNotLoggedIn");
                return false;
            }
            else
            {
                TokenString = tokenString;
                HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(tokenString);
                var response = await HttpClient.GetAsync(ISTOKENVALID_URL);
                if (response != null && response.IsSuccessStatusCode)
                {
                    SetCurrentUser(tokenString);
                    MessagingCenter.Send(this, "UserLoggedIn");
                    return true;
                }
            }
            return false;

        }

        public async Task<ObservableCollection<Car>> GetUserCars()
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(CARS_URL + CurrentUser.UserId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Car>>(result);
            }
            return null;
        }

        public async Task<ObservableCollection<Car>> GetUserCars(long userId)
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.GetAsync(CARS_URL + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Car>>(result);
            }
            return null;
        }

        public async Task<bool> AddNewCar(Car car)
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            string jsonData = JsonConvert.SerializeObject(car);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await HttpClient.PostAsync(CARS_URL + CurrentUser.UserId, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCar(long Id)
        {
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.DeleteAsync(CARS_URL + Id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUserCar(Car selectedCar)
        {
            string jsonData = JsonConvert.SerializeObject(selectedCar);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient.DefaultRequestHeaders.Authorization = await HeadersService.GetAuthenticationHeader(TokenString);
            var responseMessage = await HttpClient.PutAsync(CARS_URL + CurrentUser.UserId, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> SignUp(NewUser newUser)
        {
            string jsonData = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(SIGNUP_URL, content);
            if (response.IsSuccessStatusCode)
            {
                MessagingCenter.Send(this, "VerifyEmail");
                return true;
            }
            else
            {
                MessagingCenter.Send(this, "SignUpFailed", await response.Content.ReadAsStringAsync());
                return false;
            }

        }

        public Task Logout()
        {
            var tokenString = Preferences.Get(COOKIE_NAME, string.Empty);
            if (!tokenString.Equals(string.Empty))
            {
                Preferences.Set(COOKIE_NAME, string.Empty);
            }
            if (CurrentUser != null) CurrentUser = null;
            MessagingCenter.Send(this, "UserIsNotLoggedIn");
            return Task.FromResult(true);

        }
        private void SetCurrentUser(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(tokenString) as JwtSecurityToken;
            CurrentUser = new User()
            {
                FirstName = tokenS.Claims.First(c => c.Type.Equals("UserFirstName")).Value,
                LastName = tokenS.Claims.First(c => c.Type.Equals("UserLastName")).Value,
                UserId = long.Parse(tokenS.Claims.First(c => c.Type.Equals("UserId")).Value)
            };

        }

        public Task<User> GetCurrentUser()
        {
            return Task.FromResult(CurrentUser);
        }
    }
}
