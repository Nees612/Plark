using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plark_MobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodePage : ContentPage
    {
        private Image _image;
        public QrCodePage(ImageSource qrCode)
        {
            InitializeComponent();

            _image = new Image { Source = qrCode };

        }

        protected override void OnAppearing()
        {
            var layout = (StackLayout)FindByName("Container");
            layout.Children.Add(_image);
            base.OnAppearing();
        }
    }
}