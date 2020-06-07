using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plark_MobileClient.ViewModels
{
    public class TicketViewModel : BaseViewModel
    {
        private ObservableCollection<ArchivedTicket> _previousTickets = new ObservableCollection<ArchivedTicket>();
        private ImageSource _imageSource = null;
        private ImageSource _placeholder = ImageSource.FromUri(new Uri("https://media.ticketmaster.eu/norway/help/footerny/platinumbilletter/images/platinum1.png"));
        private string _created = null;
        private string _closed = null;
        private string _price = null;
        private int _hourlyFee = 250;
        private bool _stopped = true;
        private TimeSpan _parkingTime = new TimeSpan(0, 0, 0);
        private bool _isRefreshing = false;
        private Color _color = Color.Gray;
        private ArchivedTicket _selectedTicket;
        public Color Color { get => _color; set { SetProperty(ref _color, value); } }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }

        public ArchivedTicket SelectedTicket { get => _selectedTicket; set { SetProperty(ref _selectedTicket, value); } }
        public TicketOption TicketOption { get; set; }
        private ITicketService _ticketService => DependencyService.Get<ITicketService>();
        public ImageSource ImageSource
        {
            get => _imageSource;
            set { SetProperty(ref _imageSource, value); }
        }

        public string Created
        {
            get => _created;
            set { SetProperty(ref _created, value); }
        }
        public string Closed
        {
            get => _closed;
            set { SetProperty(ref _closed, value); }
        }
        public ObservableCollection<ArchivedTicket> PreviousTickets
        {
            get => _previousTickets;
            set { SetProperty(ref _previousTickets, value); }
        }
        public Command Refresh
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await SetPageData();

                    IsRefreshing = false;
                });
            }
        }

        public Command ImageTap
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Send(this, "ShowQr");
                });
            }
        }
        public TimeSpan ParkingTime
        {
            get => _parkingTime;
            set { SetProperty(ref _parkingTime, value); }
        }

        public string Price
        {
            get => _price;
            set { SetProperty(ref _price, value); }
        }

        public async Task<bool> GetTicket()
        {
            if (TicketOption != null)
            {
                var result = await _ticketService.GetTicket(TicketOption);
                if (result == default)
                {
                    return false;
                }
                StartTimer();
                ImageSource = result;
                Created = await _ticketService.GetCreationTime();
                Closed = await _ticketService.GetClosedTime();
                TicketOption = null;
                return true;
            }
            return false;
        }

        //public async Task<bool> CloseTicket()
        //{
        //    var result = await _ticketService.CloseTicket();
        //    if (result == default)
        //    {
        //        return false;
        //    }
        //    StopTimer();
        //    Closed = result;
        //    CalculatePrice();
        //    return true;
        //}

        private void StartTimer()
        {
            _stopped = false;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (Created != null)
                {
                    if (Closed != null)
                    {
                        var warningLimit = DateTime.Parse(Closed).Subtract(DateTime.Parse(Created));
                        var redZoneLimit = DateTime.Parse(Closed).Subtract(DateTime.Parse(Created)).Add(new TimeSpan(0, 15, 0));
                        if (ParkingTime > redZoneLimit)
                        {
                            Color = Color.Red;
                        }
                        else if (ParkingTime > warningLimit)
                        {
                            Color = Color.DarkOrange;
                        }
                        else
                        {
                            Color = Color.Gray;
                        }
                    }
                    ParkingTime = DateTime.Now.Subtract(DateTime.Parse(Created));
                }
                return !_stopped;
            });
        }

        public async Task<bool> DeleteSelectedTicket()
        {
            var result = await _ticketService.DeleteTicket(SelectedTicket.Id);

            if (result)
            {
                SelectedTicket = null;
                return true;
            }
            return false;
        }

        private void StopTimer()
        {
            _stopped = true;
        }

        public void CalculatePrice()
        {
            Price = String.Format("{0:0.00} Ft", (DateTime.Parse(Closed).Subtract(DateTime.Parse(Created)).TotalHours * _hourlyFee));
        }

        public async Task<bool> HasActiveTicket()
        {
            if (ImageSource == _placeholder)
            {
                var result = await _ticketService.HasActiveTicket();

                if (result == default)
                {
                    return false;
                }
                ImageSource = result;
                Created = await _ticketService.GetCreationTime();
                Closed = await _ticketService.GetClosedTime();
                StartTimer();
                if (Closed != null) CalculatePrice();
            }

            return true;
        }

        public async Task<bool> SetPageData()
        {
            var result = await _ticketService.GetPreviousUserTickets();

            if (ImageSource == null)
            {
                ImageSource = _placeholder;
            }
            if (result == default)
            {
                return false;
            }
            PreviousTickets = result;
            return true;
        }

        public async Task<bool> ArchiveTicket()
        {
            var result = await _ticketService.ArchiveUserTicket();
            if (result)
            {
                return true;
            }
            return false;
        }

        public void ReSetData()
        {
            ImageSource = _placeholder;
            Created = null;
            Closed = null;
            Price = null;
            Color = Color.Gray;
            ParkingTime = new TimeSpan(0, 0, 0);
        }
    }
}
