using System.Diagnostics;
using ApplicationFundamentals.View;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using System.Net.Http;
using ApplicationFundamentals.Resources.Styles;

namespace ApplicationFundamentals

{
    public partial class App : Application
    {
        private const string TestUrl = "https://www.fsafasffs.com";
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                this.Resources.MergedDictionaries.Add(new WindowsResources());
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                this.Resources.MergedDictionaries.Add(new AndroidResources());
            }
        }

        protected override async void OnStart()
        {
            var current = Connectivity.NetworkAccess;

            bool isWebsiteReachable = await IsWebsiteReachable(TestUrl);

            if (current == NetworkAccess.Internet && isWebsiteReachable)
            {
                MainPage = new StartPage();
                Debug.WriteLine("Application Started");
            }
            else
            {
                MainPage = new OfflinePage();
                Debug.WriteLine("No internet connection or website unreachable");
            }
        }   

        protected override void OnSleep()
        {
            Debug.WriteLine("Application Sleeping");
        }

        protected override void OnResume()
        {
            Debug.WriteLine("Application Resumed");
        }
        private async Task<bool> IsWebsiteReachable(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in IsWebsiteReachable: {ex.Message}");
                return false;
            }
        }

    }
}
