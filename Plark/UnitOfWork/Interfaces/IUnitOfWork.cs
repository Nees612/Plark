using Plark.Repository.Interfaces;
using Plark.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.UnitOfWorkInterfaces
{
    public interface IUnitOfWork
    {
        public ITicketRepository TicketRepository { get; }
        public IUsersRepository UsersRepoitory { get; }
        public ICarRepository CarRepository { get; }
        public IArchivedTicketRepository ArchivedTicketRepository { get; }
        public IWardenRepository WardenRepository { get; }
        Task<int> Complete();
    }
}
