using Plark.Enums;
using Plark.Models.Interface;

namespace Plark.Models
{
    public class Warden
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsEmailVerified { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public WorkPlace WorkPlace { get; set; }
    }
}
