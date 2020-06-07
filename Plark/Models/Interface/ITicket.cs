using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Models.Interface
{
    public interface ITicket
    {
        public long Id { get; set; }
        public string Token { get; set; }
    }
}
