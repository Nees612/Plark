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

        public async Task<bool> CanCreateUser(UserViewModel user)
        {
            var UserWithEmail = await _dbSet.FirstOrDefaultAsync(u => u.EmailAddress.Equals(user.EmailAddress));

            if (UserWithEmail == default)
            {
                return true;
            }

            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var User = await _dbSet.FirstOrDefaultAsync(u => u.EmailAddress.Equals(email));

            return User;
        }
    }
}
