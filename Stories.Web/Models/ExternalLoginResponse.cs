using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stories.Web.Models
{
    public class ExternalLoginResponse
    {
        public string access_token { get; set; }
        public string provider { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string state { get; set; }
        public bool hasRegistered { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
    }
}