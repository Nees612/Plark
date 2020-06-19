using Java.IO;
using Microsoft.AspNetCore.SignalR.Client;
using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plark_MobileClient.ViewModels
{
    public class TicketViewModel : BaseViewModel
    {
        private ObservableCollection<ArchivedTicket> _previousTickets = new ObservableCollection<ArchivedTicket>();
        private ImageSource _imageSource;
        private ImageSource _placeholder = "ic_local_parking_black_24dp.xml";
        private ArchivedTicket _selectedTicket;
        private TimeSpan _parkingTime = new TimeSpan(0, 0, 0);
        private Color _color;
        private int _hourlyFee = 250;
        private string _created;
        private string _closed;
        private string _price;
        private bool _stopped = true;
        private bool _isRefreshing = false;
        private bool _isActiveTicket = false;
        private HubConnection _connection;

        public ObservableCollection<ArchivedTicket> PreviousTickets { get => _previousTickets; set { SetProperty(ref _previousTickets, value); } }
        public ImageSource ImageSource { get => _imageSource; set { SetProperty(ref _imageSource, value); } }
        public TicketOption TicketOption { get; set; }
        public ArchivedTicket SelectedTicket { get => _selectedTicket; set { SetProperty(ref _selectedTicket, value); } }
        public TimeSpan ParkingTime { get => _parkingTime; set { SetProperty(ref _parkingTime, value); } }
        public Color Color { get => _color; set { SetProperty(ref _color, value); } }
        public bool IsRefreshing { get => _isRefreshing; set { SetProperty(ref _isRefreshing, value); } }
        public string Created { get => _created; set { SetProperty(ref _created, value); } }
        public string Closed { get => _closed; set { SetProperty(ref _closed, value); } }
        public string Price { get => _price; set { SetProperty(ref _price, value); } }
        public bool IsActiveTicket { get => _isActiveTicket; set { SetProperty(ref _isActiveTicket, value); } }
        private ITicketService _ticketService => DependencyService.Get<ITicketService>();
        public Command Refresh
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await SetPreviousTickets();

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

        public TicketViewModel()
        {
            ImageSource = _placeholder;
        }

        private async void OpenHubConnection()
        {
            _connection = new HubConnectionBuilder().WithUrl(EnvironmentVariables.HubUrl, options =>
            {
                options.Headers.Add("Authorization", Preferences.Get(EnvironmentVariables.COOKIE_NAME, string.Empty));
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
            }).Build();
            _connection.On("ActivatedTicket", async () =>
            {
                _imageSource = _placeholder;
                await HasActiveTicket();
                if (_connection.SendAsync("DeleteConnectionId").IsCompleted)
                {
                    await _connection.StopAsync();
                };
            });
            await _connection.StartAsync();
        }

        public async Task<bool> GetTicketIdQr()
        {
            if (TicketOption != null)
            {
                var result = await _ticketService.GetTicketIdQr(TicketOption);
                if (result == default)
                {
                    return false;
                }
                ImageSource = result;
                TicketOption = null;
                OpenHubConnection();
                return true;
            }
            return false;
        }

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
            if (!IsActiveTicket)
            {
                var result = await _ticketService.HasActiveTicket();

                if (result == default)
                {
                    IsActiveTicket = false;
                    return false;
                }
                IsActiveTicket = true;
                Created = await _ticketService.GetCreationTime();
                Closed = await _ticketService.GetClosedTime();
                ImageSource = result;
                StartTimer();
                CalculatePrice();
            }

            return true;
        }

        public async Task<bool> HasTicketIdQrImage()
        {
            if (!IsActiveTicket)
            {
                var result = await _ticketService.HasTicketIdQrImage();

                if (result != default)
                {
                    ImageSource = result;
                    return true;
                }
            }
            return false;

        }

        public async Task SetPreviousTickets()
        {
            var result = await _ticketService.GetPreviousUserTickets();

            if (result != default)
            {
                PreviousTickets = result;
            }
        }

        public async Task<bool> ArchiveTicket()
        {
            var result = await _ticketService.ArchiveUserTicket();
            if (result)
            {
                await SetPreviousTickets();
                ImageSource = _placeholder;
                IsActiveTicket = false;
                return true;
            }

            return false;
        }
    }
}
