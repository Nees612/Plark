using Plark_MobileClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Plark_MobileClient.Interface
{
    public interface IUsersService
    {
        Task<bool> IsUserLoggedIn();
        Task<bool> Login(string email, string password, bool rememberMe);
        Task<bool> SignUp(NewUser newUser);
        Task<ObservableCollection<Car>> GetUserCars();
        Task<bool> AddNewCar(Car car);
        Task<bool> DeleteCar(long Id);
        Task<bool> UpdateUserCar(Car selectedCar);
        Task<User> GetCurrentUser();
        Task Logout();
    }
}
