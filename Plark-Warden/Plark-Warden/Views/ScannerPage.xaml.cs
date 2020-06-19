using Plark_Warden.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plark_Warden.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPage : ContentPage
    {
        private ScannerViewModel _viewModel;
        public ScannerPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ScannerViewModel();
        }

        private async void ScannerButton_Clicked(object sender, EventArgs e)
        {
            var result = await _viewModel.ScanQrCode();
            if (result)
            {
                await _viewModel.ActivateTicket();
            }
        }
    }
}