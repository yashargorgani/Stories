using Stories.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Stories.Web.Helpers
{
    public static class IdentityExtensionMethods
    {
        /// <summary>
        /// to get user profile id
        /// </summary>
        public static async Task<Guid> ProfileIdAsync(this IIdentity User)
        {
            string baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];

            var model = new UserProfile();

            using (var client = new HttpClient(new AuthenticationHandler()))
            {
                client.BaseAddress = new Uri(baseAddress);

                var resp = await client.GetAsync($"UserProfile/Get");

                model = await resp.Content.ReadAsAsync<UserProfile>();
            }

            return model.Id;
        }

        public static Guid ProfileId(this IIdentity User)
        {
            string baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];

            var model = new UserProfile();

            using (var client = new HttpClient(new AuthenticationHandler()))
            {
                client.BaseAddress = new Uri(baseAddress);

                var resp = client.GetAsync($"UserProfile/Get").Result;

                model = resp.Content.ReadAsAsync<UserProfile>().Result;
            }

            return model.Id;
        }
    }
}