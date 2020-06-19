using Plark_Warden.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plark_Warden.ViewModels
{
    public class ScannerViewModel : BaseViewModel
    {
        private string _qrText;
        private bool _isTicketQrText = false;
        public string QrText { get => _qrText; set { SetProperty(ref _qrText, value); } }
        public bool IsTicketQrText { get => _isTicketQrText; set { SetProperty(ref _isTicketQrText, value); } }
        private IScannerService _scanner => DependencyService.Get<IScannerService>();
        private IJwtDecoderService _jwtDecoder => DependencyService.Get<IJwtDecoderService>();
        private ITicketService _ticketService => DependencyService.Get<ITicketService>();
        public async Task<bool> ScanQrCode()
        {
            try
            {
                var result = await _scanner.ScanAsync();
                if (result != null)
                {
                    IsTicketQrText = await _jwtDecoder.CanReadToken(result);
                    if (IsTicketQrText)
                    {
                        var claims = await _jwtDecoder.DecodeJwt(result);
                        QrText = String.Format("User Id: {0}\nCar Id: {1}\nNumberplate: {2}\nCreated: {3}\nExpires: {4}\nPrice: {5} Ft",
                            claims["UserId"],
                            claims["CarId"],
                            claims["NumberPlate"],
                            claims["Created"],
                            claims["Expires"],
                            claims["Price"]);
                    }
                    else
                    {
                        QrText = result;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> ActivateTicket()
        {
            bool result = false;
            if (!IsTicketQrText)
            {
                result = await _ticketService.ActivateTicket(long.Parse(QrText));
            }

            return result;
        }
    }
}


//                new Claim("UserId", user.Id.ToString() ),
//                new Claim("CarId", car.Id.ToString() ),
//                new Claim("NumberPlate", car.NumberPlate),
//                new Claim("Created", created.ToString()),
//                new Claim("Expires", expires.ToString()),
//                new Claim("Price", (expireTime* 250).ToString()),
