using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("auth_code"))
            {
                MainPage = new NavigationPage(new DriverHomePage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            Task.Run(async () =>
            {
                await App.Current.SavePropertiesAsync();
            });
        }

        protected override void OnResume()
        {
        }
    }
}
