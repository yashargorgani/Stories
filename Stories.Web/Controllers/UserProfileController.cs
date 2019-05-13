using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Stories.Models;
using Stories.Web.Helpers;
using Stories.Helpers;
using System.Net.Http.Formatting;

namespace Stories.Web.Controllers
{
    [GlobalAuthorize]
    public class UserProfileController : Controller
    {
        readonly string baseAddress;

        public UserProfileController()
        {
            baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            try
            {
                UserProfile model = new UserProfile();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var resp = await client.GetAsync($"UserProfile/Create");

                    model = await resp.Content.ReadAsAsync<UserProfile>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        public async Task<ActionResult> Create(UserProfile entity, string profileImg)
        {
            try
            {
                if (!string.IsNullOrEmpty(profileImg))
                {
                    string img = profileImg.Substring("data:image/jpg;base64,".Length);

                    entity.ProfileImage = Convert.FromBase64String(img);
                }

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var resp = await client.PostAsJsonAsync<UserProfile>($"UserProfile/Create", entity);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Display(Guid id)
        {
            try
            {
                var model = new UserProfile();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var resp = await client.GetAsync($"UserProfile/Get/{id}");

                    model = await resp.Content.ReadAsAsync<UserProfile>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

                //for checking if the user visiting this profile, is his/her own profile or others
                ViewBag.Owns = (await User.Identity.ProfileIdAsync()) == model.Id;

                return View(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> PartialUserProfile(Guid id)
        {
            try
            {
                var model = new UserProfile();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var resp = await client.GetAsync($"UserProfile/Get/{id}");
                    model = await resp.Content.ReadAsAsync<UserProfile>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
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