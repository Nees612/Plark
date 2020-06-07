using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plark_MobileClient.Models
{
    public class Ticket
    {
        public long Id { get; set; }
        public ImageSource ImageSource { get; set; }
        public string Creation { get; set; }
        public string Closed { get; set; }
    }
}
