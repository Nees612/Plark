using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Models
{
    public class Ticket
    {
        public long Id { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Closed { get; set; }


        public override string ToString()
        {
            return "Ticket Id" + Id.ToString() + "User Id" + User.Id + "\nFirst name:" + User.FirstName + "\nLast name:" + User.LastName
                    + "\nTicket Created at:" + Created.ToString();
        }
    }
}
