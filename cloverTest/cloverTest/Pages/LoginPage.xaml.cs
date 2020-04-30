using clearCacheLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string appId = "H4SW8FAMTYV8Y";
        string client_secret = "ad96929e-0f3d-c74c-a5b4-25ddb02d6153";
        public LoginPage()
        {
            InitializeComponent();

            //UNCOMMENT PLEASE
            DependencyService.Get<IClearCookies>().ClearAllCookies();

            webview1.Navigated += OnNavigatedHandler;


            HttpClient _client = new HttpClient();
            string url = "https://sandbox.dev.clover.com/dashboard/login?&hardRedirect=true&webRedirectUrl=https://sandbox.dev.clover.com/oauth/authorize?client_id=" + appId;
            webview1.Source = url;

        }

        public async void OnNavigatedHandler(object sender, WebNavigatedEventArgs args)
        {
            string newUrl = args.Url;
            string authCode = "";
            string merchantId = "";
            string employeeId = "";
            string token = "";
            
            if (newUrl.Contains("code="))
            {
                int authCodeIndex = newUrl.IndexOf("code=") + "code=".Length;
                authCode = newUrl.Substring(authCodeIndex);

                int midIndex = newUrl.IndexOf("merchant_id=") + "merchant_id=".Length;
                merchantId = newUrl.Substring(midIndex, 13);

                int eidIndex = newUrl.IndexOf("employee_id=") + "employee_id=".Length;
                employeeId = newUrl.Substring(eidIndex,13);

                Application.Current.Properties["auth_code"] = authCode;
                Application.Current.Properties["merchant_id"] = merchantId;
                Application.Current.Properties["employee_id"] = employeeId;

                HttpClient _client = new HttpClient();
                string getApiTokenUrl = "https://sandbox.dev.clover.com/oauth/token?client_id="+appId+"&client_secret="+client_secret+"&code=" + authCode;

                var response = await _client.GetAsync(getApiTokenUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    token = JObject.Parse(content)["access_token"].ToString();
                    Application.Current.Properties["token"] = token;
                }
                Driver driver = new Driver(null, employeeId, null, null, null, null, token , merchantId);
                currentProfile.driver = driver;
                await App.Current.SavePropertiesAsync();
                Navigation.PushAsync(new DriverHomePage());
            }
            
        }
        /*private async void AuthBtnClicked(object sender, EventArgs e)
        {
            HttpClient _client = new HttpClient(); 
            string url = "https://sandbox.dev.clover.com/dashboard/login?inactive=true&hardRedirect=true&webRedirectUrl=https://sandbox.dev.clover.com/oauth/authorize?client_id=7YEJFK5MEPNPA";
            string url1 = "https://sandbox.dev.clover.com/";
            var response = await _client.GetAsync(url1);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
            }
            webview1.Source = url;
        }*/
    }
}