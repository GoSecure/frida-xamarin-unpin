using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMulti.Services;
using XamarinMulti.Views;

namespace XamarinMulti
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
