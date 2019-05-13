using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Stories.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Stories.Web.Models;
using System.Text.RegularExpressions;
using Stories.Web.Helpers;
using System.Net.Http.Formatting;

namespace Stories.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly string baseAddress;

        public HomeController()
        {
            baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];
        }

        public async Task<ActionResult> Index()
        {
            try
            {


                return View();
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        public async Task<ActionResult> LoadStory(int page=1)
        {
            try
            {
                List<Story> model = new List<Story>();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var resp = await client.GetAsync($"Story/GetForHomePage?page={page}");
                    model = await resp.Content.ReadAsAsync<List<Story>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

                model.ForEach(
                    x => {
                        x.Body = Regex.Replace(x.Body, "<.*?>", String.Empty);
                    });

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