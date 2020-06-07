using Plark_MobileClient.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plark_MobileClient.ViewModels
{
    public class ArchivedTicketViewModel : BaseViewModel
    {
        public long _Id;
        public long _transactionId;
        public string _created;
        public string _expires;
        public string _price;
        public string _carNumberPlate;
        public string _firstName;
        public string _lastName;

        public long Id { get => _Id; set { SetProperty(ref _Id, value); } }
        public long TransactionId { get => _transactionId; set { SetProperty(ref _transactionId, value); } }
        public string Created { get => _created; set { SetProperty(ref _created, value); } }
        public string Expires { get => _expires; set { SetProperty(ref _expires, value); } }
        public string Price { get => _price; set { SetProperty(ref _price, value); } }
        public string CarNumberPlate { get => _carNumberPlate; set { SetProperty(ref _carNumberPlate, value); } }
        public string FirstName { get => _firstName; set { SetProperty(ref _firstName, value); } }
        public string LastName { get => _lastName; set { SetProperty(ref _lastName, value); } }

        private JwtSecurityToken ConvertToken(string tokenString)
        {
            var token = tokenString.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken((string)token) as JwtSecurityToken;

            return tokenS;
        }
        public void SetPageData(ArchivedTicket ticket)
        {
            var tokenS = ConvertToken(ticket.Token);
            Id = ticket.Id;
            FirstName = ticket.User.FirstName;
            LastName = ticket.User.LastName;
            Created = tokenS.Claims.First(c => c.Type.Equals("Created")).Value;
            Expires = tokenS.Claims.First(c => c.Type.Equals("Expires")).Value;
            Price = tokenS.Claims.First(c => c.Type.Equals("Price")).Value;
            CarNumberPlate = tokenS.Claims.First(c => c.Type.Equals("NumberPlate")).Value;
        }
    }
}
