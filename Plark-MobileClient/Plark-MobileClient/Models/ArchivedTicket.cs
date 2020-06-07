using System;
using System.Collections.Generic;
using System.Text;

namespace Plark_MobileClient.Models
{
    public class ArchivedTicket
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}
