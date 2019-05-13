using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Stories.Models;
using Stories.Web.Helpers;

namespace Stories.Web.Controllers
{
    public class WebLayoutController : Controller
    {
        string baseAddress;

        public WebLayoutController()
        {
            baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];
        }

        /// <summary>
        /// User profile details and favorite topics
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> PartialHeader()
        {
            try
            {
                var topics = new List<Topic>();
                var user = new UserProfile(); user = null;
                
                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);


                    var resp = await client.GetAsync($"Topic/TopicsForHeader");
                    topics = await resp.Content.ReadAsAsync<List<Topic>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

                    bool isAuth = await User.Identity.IsGlobalAuthenticateAsync();

                    if (isAuth)
                    {
                        var resp_user = await client.GetAsync($"UserProfile/Get");
                        user = await resp_user.Content.ReadAsAsync<UserProfile>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                    }
                }

                var model = new Tuple<List<Topic>, UserProfile>(topics, user);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                throw ex;
            }
        }

    }
}