using Plark_MobileClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plark_MobileClient.Interface
{
    public interface ITicketManager
    {
        Task<Ticket> DecodeQrCode(ImageSource imageSource);
    }
}
