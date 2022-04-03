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
    [GlobalAuthorize]
    [AllowAnonymous]
    public class TopicController : Controller
    {
        string baseAddress;

        public TopicController()
        {
            baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Select()
        {
            try
            {
                var allTopics = new List<Topic>();
                var selectedTopics = new List<Topic>();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var respAllTopics = await client.GetAsync($"Topic/AllTopics");
                    allTopics = await respAllTopics.Content.ReadAsAsync<List<Topic>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

                    var respSelectedTopics = await client.GetAsync($"Topic/SelectedTopics");
                    selectedTopics = await respSelectedTopics.Content.ReadAsAsync<List<Topic>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

                var model = new List<SelectListItem>();

                foreach (var x in allTopics)
                {
                    model.Add(new SelectListItem()
                    {
                        Text = x.Caption,
                        Value = x.Id.ToString(),
                        Selected = selectedTopics.Select(y => y.Id).Contains(x.Id)
                    });                    
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Select(FormCollection collection)
        {
            try
            {
                List<int> selectedItems = new List<int>();

                if (!string.IsNullOrEmpty(collection["Topics"]))
                {
                    foreach (var x in collection["Topics"].Split(','))
                    {
                        selectedItems.Add(int.Parse(x));
                    }
                }

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var resp = await client.PostAsJsonAsync<List<int>>($"Topic/Select", selectedItems);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        public async Task<ActionResult> PartialUserTopics(Guid UserProfileId)
        {
            try
            {
                var model = new List<Topic>();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var respSelectedTopics = await client.GetAsync($"Topic/SelectedTopics?UserProfileId={UserProfileId}");
                    model = await respSelectedTopics.Content.ReadAsAsync<List<Topic>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

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