using cloverTest.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverHomePage : ContentPage
    {
        //public List<Trip> trips;
        string merchantId = "";
        string employeeId = "";
        string token = "";
        //Driver mDriver;
        string baseURL = "https://sandbox.dev.clover.com";

        ObservableCollection<Trip> trips = new ObservableCollection<Trip>();
        public ObservableCollection<Trip> Trips { get { return trips; } }
        public DriverHomePage()
        {
            InitializeComponent();

            //trips = new List<Trip>();
            token = Application.Current.Properties["token"].ToString(); ;
            merchantId = Application.Current.Properties["merchant_id"].ToString(); ;
            employeeId = Application.Current.Properties["employee_id"].ToString(); ;


            JObject jsonObj = sendGet("/v3/merchants/" + merchantId + "/employees/" + employeeId);
            currentProfile.driver = JsonConvert.DeserializeObject<Driver>(jsonObj.ToString());
            currentProfile.driver.token = token;
            currentProfile.driver.merchantID = merchantId;

            JObject ordersObj = sendGet("/v3/merchants/" + merchantId + "/employees/" + employeeId + "/orders");
            JObject paymentsObj = sendGet("/v3/merchants/" + merchantId + "/employees/" + employeeId + "/payments");

            var elements = paymentsObj.GetValue("elements");
            var elementsList = paymentsObj.GetValue("elements").ToList();

            //Payments p1 = JsonConvert.DeserializeObject<Payments>(paymentsObj.ToString());
            List<Payments> p2 = JsonConvert.DeserializeObject<List<Payments>>(elements.ToString());

            trips = JsonConvert.DeserializeObject<ObservableCollection<Trip>>(elements.ToString());
            List<string> amounts = new List<string>();

            for (int i = 0; i < elements.Count(); i++)
            {
                //Payments p2 = paymentsObj[i].ToObject<Payments>();
                var element = elements[i];
                Payments p = element.ToObject<Payments>();
                trips[i].orderID = p2[i].order.id;
                //trips[i].currency p2[i].c;
                trips[i].startTime = p2[i].clientCreatedTime;
                trips[i].endTime = p2[i].modifiedTime;
                trips[i].tripName = "Trip " + (i+1);
                //amounts.Add(trips[i].total);
            }
            amounts.Reverse();
            //trips.Reverse();
            trips = new ObservableCollection<Trip>(trips.Reverse());
            BindingContext = this;

            //tripsListview.ItemsSource = amounts;
        }

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
            return jsonObj;
        }
        #endregion

        private void goToProfileBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DriverProfilePage());
        }


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

        private void addNewTripBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewTripPage("Trip "+Trips.Count+1));
        }

        private void onTripClicked(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new TripDetailsPage(trips[e.SelectedItemIndex]));
        }
    }
}