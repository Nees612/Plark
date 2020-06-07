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
    public class ArchivedTicketRepository : Repository<ArchivedTicket>, IArchivedTicketRepository
    {
        public ArchivedTicketRepository(PlarkContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ArchivedTicket>> GetTicketsByUserId(long userId)
        {
            var tickets = await _dbSet.Where(t => t.User.Id.Equals(userId)).Select(t => new ArchivedTicket
            {
                Id = t.Id,
                TransactionId = t.TransactionId,
                User = new User { Id = t.User.Id, FirstName = t.User.FirstName, LastName = t.User.LastName },
                Token = t.Token
            }).ToListAsync();

            return tickets;
        }
    }
}
