using clearCacheLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverProfilePage : ContentPage
    {
        //string authCode = "";
        string merchantId = "";
        string employeeId = "";
        string token = "";
        Driver mDriver;
        string baseURL = "https://sandbox.dev.clover.com";


        #region Constructors
        public DriverProfilePage()
        {
            InitializeComponent();

            merchantId = Application.Current.Properties["merchant_id"].ToString();
            employeeId = Application.Current.Properties["employee_id"].ToString();
            if (Application.Current.Properties.ContainsKey("token"))
            {
                token = Application.Current.Properties["token"].ToString();
            }
            JObject jsonObj = sendGet("/v3/merchants/" + merchantId + "/employees/" + employeeId);
            mDriver = JsonConvert.DeserializeObject<Driver>(jsonObj.ToString());

            mDriver.token = token;
            mDriver.merchantID = merchantId;
            
            idLabel.Text = mDriver.id;
            nicknameLabel.Text = mDriver.nickname;
            emailLabel.Text = mDriver.email;
            pinLabel.Text = mDriver.pin;
            roleLabel.Text = mDriver.role;
            label.Text = "Welcome " + mDriver.name + "!";
        }
        #endregion


        #region Buttons
        private async void LogoutBtnClicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Remove("auth_code");
            Application.Current.Properties.Remove("merchant_id");
            Application.Current.Properties.Remove("employee_id");
            Application.Current.Properties.Remove("token");

            await App.Current.SavePropertiesAsync();
            Navigation.PushAsync(new MainPage());
        }
        #endregion

        #region Get and Post Requests
        private JObject sendGet(String endpoint)
        {
            string url = baseURL + endpoint + "?access_token=" + token;

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            JObject jsonObj = new JObject();
            if (response.IsSuccessful)
            {
                var content = response.Content;
                jsonObj = JObject.Parse(content);
            }
            return jsonObj ;
        }


        private JObject sendPost(String endpoint, JsonObject obj)
        {
            string url = baseURL + endpoint;
            url += "?access_token=" + token;

            var client = new RestClient(url);

            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");

            request.AddParameter("application/json", obj.ToString() , ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject jsonObj = new JObject();
            if (response.IsSuccessful)
            {
                var content = response.Content;
                jsonObj = JObject.Parse(content);
            }
            return jsonObj;
        }

        #endregion

        private void editBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DriverProfileEditorPage());

        }
    }
}