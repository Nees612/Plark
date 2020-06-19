using Plark.Context;
using Plark.Repository;
using Plark.Repository.Interfaces;
using Plark.RepositoryInterfaces;
using Plark.UnitOfWorkInterfaces;
using System.Threading.Tasks;

namespace Plark.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlarkContext _plarkContext;
        public UnitOfWork(PlarkContext plarkContext)
        {
            _plarkContext = plarkContext;
            UsersRepoitory = new UsersRepository(_plarkContext);
            TicketRepository = new TicketRepository(_plarkContext);
            CarRepository = new CarRepository(_plarkContext);
            ArchivedTicketRepository = new ArchivedTicketRepository(_plarkContext);
            WardenRepository = new WardenRepository(_plarkContext);
        }
        public IUsersRepository UsersRepoitory { get; private set; }
        public ITicketRepository TicketRepository { get; private set; }
        public ICarRepository CarRepository { get; private set; }
        public IArchivedTicketRepository ArchivedTicketRepository { get; private set; }
        public IWardenRepository WardenRepository { get; private set; }

        public async Task<int> Complete()
        {
            return await _plarkContext.SaveChangesAsync();
        }
    }
}
