
using Android.Graphics;
using Plark_MobileClient.Interface;
using Plark_MobileClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;

namespace Plark_MobileClient.Managers
{
    public class TicketManager : ITicketManager
    {
        //public Task<Ticket> DecodeQrCode(ImageSource imageSource)
        //{
        //    Task<Bitmap> GetBitmap(Xamarin.Forms.Image image)
        //    {
        //        BarcodeReader Reader = new ZXing.Presentation.BarcodeReader();

        //        if (frameHolder.Source != null)
        //        {
        //            Result result = Reader.Decode((BitmapSource)frameHolder.Source);
        //            decoded = result?.Text;
        //            hey.Text = decoded;
        //        }
        //    }
        //}
        public Task<Ticket> DecodeQrCode(ImageSource imageSource)
        {
            throw new NotImplementedException();
        }
    }
}
