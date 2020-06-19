using Plark.Models;
using Plark.Models.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Managers.Interfaces
{
    public interface ITicketManager
    {
        Task<byte[]> BitmapToBytes(Bitmap img);
        Task<Bitmap> CreateQrcode(string data);
        Task<string> GenerateTicketJwtToken(User user, Car car, double expireTime);
        Task<string> GetCreatedFromToken(string token);
        Task<string> GetExpireFromToken(string token);

    }
}
