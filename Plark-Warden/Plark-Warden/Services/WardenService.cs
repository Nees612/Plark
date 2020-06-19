using Newtonsoft.Json;
using Plark_Warden.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plark_Warden.Services
{
    public class WardenService : Service
    {
        //public async Task<bool> SignUp(NewWarden newWarden)
        //{
        //    string jsonData = JsonConvert.SerializeObject(newWarden);
        //    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        //    var response = await HttpClient.PostAsync(SIGNUP_URL, content);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        MessagingCenter.Send(this, "VerifyEmail");
        //        return true;
        //    }
        //    else
        //    {
        //        MessagingCenter.Send(this, "SignUpFailed", await response.Content.ReadAsStringAsync());
        //        return false;
        //    }

        //}

        public Task Logout()
        {
            var tokenString = Preferences.Get(EnvironmentVariables.COOKIE_NAME, string.Empty);
            if (!tokenString.Equals(string.Empty))
            {
                Preferences.Set(EnvironmentVariables.COOKIE_NAME, string.Empty);
            }
            if (CurrentWarden != null) CurrentWarden = null;
            MessagingCenter.Send(this, "UserIsNotLoggedIn");
            return Task.FromResult(true);

        }
        private void SetCurrentUser(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(tokenString) as JwtSecurityToken;
            CurrentWarden = new Warden()
            {
                FirstName = tokenS.Claims.First(c => c.Type.Equals("FirstName")).Value,
                LastName = tokenS.Claims.First(c => c.Type.Equals("LastName")).Value,
                WardenId = long.Parse(tokenS.Claims.First(c => c.Type.Equals("Id")).Value)
            };

        }

        public Task<Warden> GetCurrentUser()
        {
            return Task.FromResult(CurrentWarden);
        }
    }
}
