using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;


namespace Stories.Service.Models
{
    public class GoogleUserOutputData
    {
        /// <summary>
        /// getting user info from GOOGLE by access token
        /// </summary>
        public static async Task<GoogleUserOutputData> GetUserByTokenAsync(string accessToken)
        {
            HttpClient client = new HttpClient();

            var urlProfile = $"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}";

            client.CancelPendingRequests();

            HttpResponseMessage output = await client.GetAsync(urlProfile);

            if (!output.IsSuccessStatusCode)
            {
                return null;
            }

            GoogleUserOutputData userData = await output.Content.ReadAsAsync<GoogleUserOutputData>();

            return userData;
        }

        public string id { get; set; }

        public string name { get; set; }

        public string given_name { get; set; }

        public string email { get; set; }

        public string picture { get; set; }
    }
}