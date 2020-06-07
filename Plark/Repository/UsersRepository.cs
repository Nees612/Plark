using Microsoft.EntityFrameworkCore;
using Plark.Context;
using Plark.Models;
using Plark.RepositoryInterfaces;
using Plark.ViewModels;
using System.Threading.Tasks;

namespace Plark.Repository
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(PlarkContext plarkContext) : base(plarkContext) { }

        public async Task<bool> IsUserExistWithPhoneNumber(string phoneNumber)
        {
            var UserWithPhoneNumber = await _dbSet.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(phoneNumber));

            if (UserWithPhoneNumber == default)
            {
                return false;
            }

            return true;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var User = await _dbSet.FirstOrDefaultAsync(u => u.EmailAddress.Equals(email));

            return User;
        }

        public async Task<User> GetUserByIdWithCars(long id)
        {
            var User = await _dbSet.Include(u => u.Cars).FirstOrDefaultAsync(u => u.Id.Equals(id));

            return User;
        }
    }
}
