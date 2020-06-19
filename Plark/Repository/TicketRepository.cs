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

        public Task<Ticket> GetTicketByUser(User user)
        {
            var ticket = _dbSet.FirstOrDefault(t => t.User.Equals(user));
            return Task.FromResult(ticket);
        }

        public Task<Ticket> GetTicketWithCarAndUser(long ticketId)
        {
            var ticket = _dbSet.Include(t => t.Car).Include(t => t.User).FirstOrDefault(t => t.Id.Equals(ticketId));
            return Task.FromResult(ticket);
        }
    }
}
