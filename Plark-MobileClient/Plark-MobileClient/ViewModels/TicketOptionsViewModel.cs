using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plark_MobileClient.ViewModels
{
    public class TicketOptionsViewModel : BaseViewModel
    {
        private IUsersService _usersService => DependencyService.Get<IUsersService>();

        private ObservableCollection<Car> _cars = new ObservableCollection<Car>();
        public ObservableCollection<Car> Cars
        {
            get => _cars;
            set
            {
                SetProperty(ref _cars, value);
            }
        }

        private ObservableCollection<TimeInterval> _timeIntervals = new ObservableCollection<TimeInterval>();
        public ObservableCollection<TimeInterval> TimeIntervals
        {
            get => _timeIntervals;
            set
            {
                SetProperty(ref _timeIntervals, value);
            }
        }

        public Car SelectedCar { get; set; }
        public TimeInterval SelectedInterval { get; set; }

        public async Task<bool> SetPageData()
        {
            TimeIntervals = new ObservableCollection<TimeInterval>()
            {
                new TimeInterval {Interval = new TimeSpan(1,0,0) },
                new TimeInterval {Interval = new TimeSpan(1,30,0) },
                new TimeInterval {Interval = new TimeSpan(2,0,0) },
                new TimeInterval {Interval = new TimeSpan(2,30,0) },
                new TimeInterval {Interval = new TimeSpan(3,0,0) },
                new TimeInterval {Interval = new TimeSpan(3,30,0) },
                new TimeInterval {Interval = new TimeSpan(4,0,0) },
                new TimeInterval {Interval = new TimeSpan(4,30,0) },
                new TimeInterval {Interval = new TimeSpan(5,0,0) }
            };

            Cars = await _usersService.GetUserCars();

            if (Cars != null)
            {
                return true;
            }
            return false;
        }
    }
}
