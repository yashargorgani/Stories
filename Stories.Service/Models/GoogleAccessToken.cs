using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Stories.Service.Models
{
    public class GoogleAccessToken
    {
        public static async Task<GoogleAccessToken> GetTokenByCodeAsync(string code)
        {
            string googleAuthUrl = "https://accounts.google.com/o/oauth2/";
            string client_id = ConfigurationManager.AppSettings["GoogleClientId"];
            string client_secret = ConfigurationManager.AppSettings["GoogleClientSecret"];
            string returnUrl = ConfigurationManager.AppSettings["GoogleReturnUrl"];

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create($"{googleAuthUrl}token");

            webRequest.Method = "POST";

            string parameters = $"code={code}&client_id={client_id}&client_secret={client_secret}&redirect_uri={returnUrl}&grant_type=authorization_code";

            byte[] byteArray = Encoding.UTF8.GetBytes(parameters);

            webRequest.ContentType = "application/x-www-form-urlencoded";

            webRequest.ContentLength = byteArray.Length;

            Stream postStream = await webRequest.GetRequestStreamAsync();

            await postStream.WriteAsync(byteArray, 0, byteArray.Length);

            postStream.Close();

            WebResponse response = await webRequest.GetResponseAsync();

            postStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(postStream);

            string responseFromServer = await reader.ReadToEndAsync();

            GoogleAccessToken googleAccessToken = JsonConvert.DeserializeObject<GoogleAccessToken>(responseFromServer);

            return googleAccessToken;
        }

        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires_in { get; set; }

        public string id_token { get; set; }

        public string refresh_token { get; set; }
        public string email { get; set; }

        public string returnUrl { get; set; }
    }
}