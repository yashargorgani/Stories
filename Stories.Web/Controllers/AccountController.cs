using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;
using Stories.Web.Helpers;
using System.Net.Http.Formatting;
using System.Web.Configuration;

namespace Stories.Web.Controllers
{
    public class AccountController : Controller
    {
        private HttpClient client;

        public AccountController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);
        }

        public async Task<ActionResult> Login()
        {
            try
            {
                var resp = await client.GetAsync($"Account/GetWebLoginUri");

                var url = await resp.Content.ReadAsAsync<string>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

                return Redirect(url);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        public ActionResult Logedin(string access_token, string email, int expires_in, bool isRegistered)
        {
            try
            {
                FormsAuthentication.SetAuthCookie(email, false);

                Response.Cookies.Add(new HttpCookie("accessToken")
                {
                    Value = access_token,
                    Expires = DateTime.Now.AddSeconds(expires_in)
                });

                if (!isRegistered)
                    return RedirectToAction("Select", "Topic");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Index", "Home");
        }
    }
}