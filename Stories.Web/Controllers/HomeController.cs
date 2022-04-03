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

        public async Task<ActionResult> Index(SearchParam sp)
        {
            try
            {
                return View(await _GetStories(sp));
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        public async Task<ActionResult> LoadStory(SearchParam sp)
        {
            try
            {
                var model = await _GetStories(sp);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                throw ex;
            }
        }

        private async Task<IEnumerable<Story>> _GetStories(SearchParam sp)
        {
            List<Story> model = new List<Story>();

            using (var client = new HttpClient(new AuthenticationHandler()))
            {
                client.BaseAddress = new Uri(baseAddress);
                string url = $"Story/GetForHomePage{sp.ToUrl()}";
                //if(!string.IsNullOrEmpty(q))
                //    url = url + "&q=" + q;
                var resp = await client.GetAsync(url);
                //var json = await resp.Content.ReadAsStringAsync();
                //model = JsonConvert.DeserializeObject<List<Story>>(json);

                if(resp.IsSuccessStatusCode)
                    model = await resp.Content.ReadAsAsync<List<Story>>();
            }

            model.ForEach(
                x =>
                {
                    x.Body = Regex.Replace(x.Body, "<.*?>", String.Empty);
                });

            return model;
        }
    }
}