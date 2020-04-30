using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        string merchantId ;
        string employeeId ;
        string token ;
        string baseURL = "https://sandbox.dev.clover.com";
        Trip mTrip;
        //Double amount;
        public PaymentPage(Trip trip)
        {
            InitializeComponent();
            mTrip = trip;
            amountLabel.Text = mTrip.total.ToString();
             merchantId = currentProfile.driver.merchantID;
             employeeId = currentProfile.driver.id;
             token = currentProfile.driver.token;
        }

        private void PayBtnClicked(object sender, EventArgs e)
        {
            string orderID = createOrder(0);

            JObject paymentKeys = sendGet("/v2/merchant/" + merchantId + "/pay/key");

            string modulus = paymentKeys["modulus"].ToString();
            string exponent = paymentKeys["exponent"].ToString();
            string prefix = paymentKeys["prefix"].ToString();
            //string CCNumber = "6011361000006668";
            //string CCNumber = "4005578003333335";
            //Double amount = Convert.ToDouble(amountEntry.Text);
            //amount = mTrip.total;
            string CCNumber = ccnumber.Text;
            string encryptedCC = EncryptCreditCard(modulus, exponent, prefix, CCNumber);

            JsonObject payment = new JsonObject();
            payment.Add("orderId", orderID);
            payment.Add("currency", "usd");
            payment.Add("expMonth", Convert.ToInt32(expMonthEntry.Text));
            payment.Add("cvv", Convert.ToInt32(cvvEntry.Text));
            payment.Add("expYear", Convert.ToInt32(expYearEntry.Text));
            payment.Add("cardEncrypted", encryptedCC);
            payment.Add("amount", mTrip.total);
            payment.Add("last4", CCNumber.Substring(CCNumber.Length - 4, 4));
            payment.Add("first6", CCNumber.Substring(0, 6));

            JObject paymentObj = sendPost("/v2/merchant/" + merchantId + "/pay", payment);
            if (paymentObj["result"].Equals("APPROVED"))
            {
                mTrip.state = "locked";
            }
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


        private JObject sendPost(String endpoint, JsonObject obj)
        {
            string url = baseURL + endpoint;
            url += "?access_token=" + token;

            var client = new RestClient(url);

            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");

            request.AddParameter("application/json", obj.ToString(), ParameterType.RequestBody);
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

        #region Payment Functions
        public string createOrder(int amount)
        {
            JsonObject order = new JsonObject();
            JsonObject employee = new JsonObject();
            employee.Add("id", employeeId);
            order.Add("employee", employee);
            order.Add("total", amount);

            JObject jsonObj = sendPost("/v3/merchants/" + merchantId + "/orders", order);
            return jsonObj.GetValue("id").ToString();

            /*var client = new RestClient("https://sandbox.dev.clover.com/v3/merchants/" + mid + "/orders?access_token=" + token);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"employee\":{\"id\":\"" + eid + "\"},\"total\":100}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var content = response.Content;
                JObject jsonObj = JObject.Parse(content);

                return jsonObj.GetValue("id").ToString();
            }
            return "";
            */
        }

        private string EncryptCreditCard(string modulus, string exponent, string prefix, string ccNumber)
        {
            byte[] modulusBytes = BigInteger.Parse(modulus).ToByteArray();
            byte[] exponentBytes = BitConverter.GetBytes(Int64.Parse(exponent));

            // If the byte arrays are little-endian, reverse them to become
            // big-endian to be accepted as RSA parameters.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(modulusBytes);
                Array.Reverse(exponentBytes);
            }
            RSAParameters rsaParams = new RSAParameters
            {
                Modulus = modulusBytes,
                Exponent = exponentBytes
            };
            RSA rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);

            byte[] encryptedCard = rsa.Encrypt(Encoding.UTF8.GetBytes(prefix + ccNumber),
                                               RSAEncryptionPadding.OaepSHA1);

            string base64EncryptedCard = Convert.ToBase64String(encryptedCard);
            return base64EncryptedCard;
        }
        #endregion

    }
}