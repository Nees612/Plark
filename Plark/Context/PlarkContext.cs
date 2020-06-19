using Microsoft.EntityFrameworkCore;
using Plark.Models;

namespace Plark.Context
{
    public class PlarkContext : DbContext
    {
        public PlarkContext(DbContextOptions<PlarkContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<ArchivedTicket> ArchivedTickets { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Warden> Wardens { get; set; }

    }
}
