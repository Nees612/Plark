using Plark.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Plark.Models
{
    public class ArchivedTicket : ITicket
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}
