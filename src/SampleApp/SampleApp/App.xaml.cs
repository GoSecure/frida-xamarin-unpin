using System.Net.Http;
using Xamarin.Forms;
using SampleApp.Services;
using SampleApp.Views;

namespace SampleApp
{
    public partial class App : Application
    {
        public static ILogger Log;
        public static HttpClient Http;
        public static bool Pinning;

        public App(ILogger logger, HttpClient http = null)
        {
            InitializeComponent();
            Log = logger;
            Http = http ?? new HttpClient();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
