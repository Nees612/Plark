using Plark.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Models
{
    public class Ticket : ITicket
    {
        public long Id { get; set; }
        public User User { get; set; }
        public string Token { get; set; }

    }
}
