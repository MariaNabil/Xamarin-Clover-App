using System;
using System.Collections.Generic;
using System.Text;

namespace cloverTest
{
    public class Payments
    {
        public string id { get; set; }
        public Order order { get; set; }
        public Tender tender { get; set; }
        public int amount { get; set; }
        public int taxAmount { get; set; }
        public int cashBackAmount { get; set; }
        public Employee employee { get; set; }
        public string clientCreatedTime { get; set; }
        public string createdTime { get; set; }
        public string modifiedTime { get; set; }
        public Boolean offline { get; set; }
        public string result { get; set; }

    }
}
