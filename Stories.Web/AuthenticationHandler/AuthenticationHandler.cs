using Stories.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Stories.Web
{
    public class AuthenticationHandler : HttpClientHandler
    {
        private string email;
        private string accessToken;
        private string _token;

        public AuthenticationHandler()
        {
            accessToken = "DEFAULT";

            HttpCookie cookie = HttpContext.Current.Request.Cookies["accessToken"];
            if (cookie != null)
                accessToken = cookie.Value;

            email = "DEFAULT";

            if (HttpContext.Current.User.Identity.IsAuthenticated)
                email = HttpContext.Current.User.Identity.Name;

            this._token = Convert.ToBase64String(Encoding.UTF8.GetBytes(email + ":" + accessToken));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationTocke)
        {
            request.Headers.Add("Authorization", "Basic " + _token);

            return base.SendAsync(request, cancellationTocke);
        }
    }

    /// <summary>
    /// the simple Identity.IsAuthenticated on check the cookie.
    /// here in web project we don't have access to db, so existing and validating a user is done through web api
    /// so this method checks the user via web api
    /// </summary>
    public static class AuthenticationExtensions
    {
        public static bool IsGlobalAuthenticate(this IIdentity identity)
        {
            if (!identity.IsAuthenticated)
                return false;

            string baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];

            UserAccount account;

            using (var client = new HttpClient(new AuthenticationHandler()))
            {
                client.BaseAddress = new Uri(baseAddress);

                var response = client.GetAsync($"Account/GetCurrentUser").Result;

                if (!response.IsSuccessStatusCode)
                    return false;

                account = response.Content.ReadAsAsync<UserAccount>().Result;
            }

            if (account == null)
                return false;

            return account.EmailAddress == identity.Name;
        }

        public static async Task<bool> IsGlobalAuthenticateAsync(this IIdentity identity)
        {
            if (!identity.IsAuthenticated)
                return false;

            string baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];

            UserAccount account;

            using (var client = new HttpClient(new AuthenticationHandler()))
            {
                client.BaseAddress = new Uri(baseAddress);

                var response = await client.GetAsync($"Account/GetCurrentUser");

                if (!response.IsSuccessStatusCode)
                    return false;

                account = await response.Content.ReadAsAsync<UserAccount>();
            }

            if (account == null)
                return false;

            return account.EmailAddress == identity.Name;
        }
    }
}