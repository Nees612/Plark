using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Plark.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public bool IsEmailVerified { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Text)]
        [StringLength(60)]
        public string PasswordHash { get; set; }
        public string ConnectionId { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
