using Plark.Models;
using Plark.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.RepositoryInterfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<bool> IsUserExistWithPhoneNumber(string phoneNumber);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByIdWithCars(long id);
    }
}
