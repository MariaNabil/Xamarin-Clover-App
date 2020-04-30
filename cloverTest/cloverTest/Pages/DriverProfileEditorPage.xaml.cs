using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverProfileEditorPage : ContentPage
    {
        //Driver mDriver;
        string baseURL = "https://sandbox.dev.clover.com";

        public DriverProfileEditorPage()
        {
            InitializeComponent();
            //mDriver = driver;
            //nameEntry.Text = mDriver.name;
            nicknameEntry.Text = currentProfile.driver.nickname;
            //emailEntry.Text = mDriver.email;
            pinEntry.Text = currentProfile.driver.pin;
            name.Text = currentProfile.driver.name + " Profile";
            //roleEntry.Text = mDriver.role;
        }

        private void doneBtnClicked(object sender, EventArgs e)
        {
            JsonObject employee = new JsonObject();
            //employee.Add("name", currentProfile.driver.name);
            employee.Add("nickname", nicknameEntry.Text);
            //employee.Add("email", currentProfile.driver.email);
            employee.Add("pin", pinEntry.Text);
            //employee.Add("role", currentProfile.driver.role);

            var s = sendPost("/v3/merchants/" + currentProfile.driver.merchantID +"/employees/" + currentProfile.driver.id, employee);

            if(s != null)
            {
                currentProfile.driver.nickname = nicknameEntry.Text;
                currentProfile.driver.pin = pinEntry.Text;
            }
            Navigation.PushAsync(new DriverProfilePage());

        }

        private JObject sendPost(String endpoint, JsonObject obj)
        {
            string url = baseURL + endpoint;
            url += "?access_token=" + currentProfile.driver.token;
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddParameter("application/json", obj.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject jsonObj = null;
            if (response.IsSuccessful)
            {
                var content = response.Content;
                jsonObj = JObject.Parse(content);
            }
            return jsonObj;
        }
    }
}