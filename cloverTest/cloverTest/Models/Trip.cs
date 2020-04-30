using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloverTest
{
    public class Trip
    {
        public String tripName { get; set; }
        public Double baseFares { get; set; }
        public Double tolls { get; set; }
        public Double tips { get; set; }
        public Double processingFees { get; set; }
        [JsonProperty("amount")]
        public Double total { get; set; }
        public String receiptUrl { get; set; }
        public String currency { get; set; }
        public String state { get; set; }
        public String customerMail { get; set; }
        public String customerNumber { get; set; }
        public String orderID { get; set; }
        [JsonProperty("id")]
        public String paymentID { get; set; }
        public String startTime { get; set; }
        public String endTime { get; set; }

        public Trip()
        {
        }

        public Trip(string tripName, Double baseFares, Double tolls, Double tips, Double processingFees, Double total, string receiptUrl, string currency, string state, string customerMail, string customerNumber, string orderID, string paymentID, string startTime, string endTime)
        {
            this.tripName = tripName;
            this.baseFares = baseFares;
            this.tolls = tolls;
            this.tips = tips;
            this.processingFees = processingFees;
            this.total = total;
            this.receiptUrl = receiptUrl;
            this.currency = currency;
            this.state = state;
            this.customerMail = customerMail;
            this.customerNumber = customerNumber;
            this.orderID = orderID;
            this.paymentID = paymentID;
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
}
