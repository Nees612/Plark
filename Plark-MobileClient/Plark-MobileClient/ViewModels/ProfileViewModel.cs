using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using Plark_MobileClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Plark_MobileClient.ViewModels
{
    class ProfileViewModel : BaseViewModel
    {
        private IUsersService _usersService => DependencyService.Get<IUsersService>();

        private string _name;
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }
        private ObservableCollection<Car> _cars = new ObservableCollection<Car>();
        public ObservableCollection<Car> Cars
        {
            get => _cars;
            set
            {
                SetProperty(ref _cars, value);
            }
        }

        public Command Refresh
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await SetViewData();

                    IsRefreshing = false;
                });
            }
        }
        public Car SelectedCar { get; set; }
        public string Name { get => _name; set { SetProperty(ref _name, value); } }
        public async void Logout()
        {
            await _usersService.Logout();
        }

        public async Task<bool> SetViewData()
        {
            var User = await _usersService.GetCurrentUser();
            Name = User.FirstName + ' ' + User.LastName;
            Cars = await _usersService.GetUserCars();
            return true;
        }

        public async Task<bool> AddNewCar(string numberPlate)
        {
            var car = new Car { NickName = "MyCar", NumberPlate = numberPlate };
            var result = await _usersService.AddNewCar(car);
            if (result) Cars = await _usersService.GetUserCars();

            return result;
        }

        public async Task<bool> DeleteCar()
        {
            var result = await _usersService.DeleteCar(SelectedCar.Id);
            if (result) _cars = await _usersService.GetUserCars();

            return result;
        }

        public async Task<bool> UpdateSelectedCar(Car car)
        {
            var result = await _usersService.UpdateUserCar(car);

            if (result)
            {
                _cars[_cars.IndexOf(SelectedCar)] = car;
                return result;
            }
            return result;
        }

    }

}

