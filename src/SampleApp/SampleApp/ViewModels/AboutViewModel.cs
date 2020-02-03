using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SampleApp.ViewModels
{
    public class AboutViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public AboutViewModel()
        {
            Title = "About";
            MakeHttpRequest = new Command(MakeHttpRequestImpl);
            TogglePinningCommand = new Command(TogglePinning);
            ClearLogs = new Command(DoClearLogs);
            TogglePinning(null);
        }


        private void TogglePinning(object sender)
        {
            App.Pinning = !App.Pinning;

            LogOutput += DateTime.Now.ToString() + $"> Pinning: {(App.Pinning ? "ON" : "OFF")}\n";
            App.Log.WriteLine("Pinning State Changed: " + App.Pinning.ToString());

        }

        private async void MakeHttpRequestImpl(object obj)
        {
            RequestStatus = "Starting Request";
            LogOutput += DateTime.Now.ToString() + "> Making Request\n";
            try
            {
                var r = await App.Http.GetAsync("https://www.segfault.me?k=" + DateTime.Now.Ticks.ToString());
                RequestStatus = "Return Code: " + r.StatusCode.ToString();
                LogOutput += DateTime.Now.ToString() + "> Got Response: " + RequestStatus + "\n";

            }
            catch (Exception e)
            {
                RequestStatus = "Exception: " + e.Message;
                LogOutput += DateTime.Now.ToString() + "> Got Exception: " + e.Message + "\n";
            }
        }

        private void DoClearLogs(object obj)
        {
            LogOutput = "";
        }


        public ICommand MakeHttpRequest { get; }
        public ICommand TogglePinningCommand { get; }
        public ICommand ClearLogs { get; }

        private string _reqStat;
        public string RequestStatus
        {
            get { return _reqStat; }
            set
            {
                _reqStat = value;
                OnPropertyChanged(nameof(RequestStatus));
            }
        }


        private string _logs;
        public string LogOutput
        {
            get
            { return _logs; }

            set
            {
                _logs = value;
                OnPropertyChanged(nameof(LogOutput));

            }
        }
    }
}
