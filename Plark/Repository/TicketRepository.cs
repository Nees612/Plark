using Microsoft.EntityFrameworkCore;
using Plark.Context;
using Plark.Models;
using Plark.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(PlarkContext context) : base(context) { }
    }
}
