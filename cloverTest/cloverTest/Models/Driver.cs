using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloverTest
{
    public class Driver 
    {
        public string name;
        public string id;
        public string nickname;
        public string email;
        public string pin;
        public string role;
        public string isOwner;
        public string token;
        public string merchantID;


        public Driver()
        {
        }

        public Driver(string name, string id, string nickname, string pin, string role, string isOwner, string token, string merchantID)
        {
            this.name = name;
            this.id = id;
            this.nickname = nickname;
            this.pin = pin;
            this.role = role;
            this.isOwner = isOwner;
            this.token = token;
            this.merchantID = merchantID;
        }

    }
}
