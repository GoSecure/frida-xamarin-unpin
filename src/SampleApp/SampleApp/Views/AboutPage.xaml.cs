using System.ComponentModel;
using Xamarin.Forms;
using SampleApp.ViewModels;

namespace SampleApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        AboutViewModel viewModel = new AboutViewModel();

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}