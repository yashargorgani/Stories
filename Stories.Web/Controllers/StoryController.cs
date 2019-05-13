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
    [GlobalAuthorize]
    public class StoryController : Controller
    {
        readonly string baseAddress;

        public StoryController()
        {
            baseAddress = ConfigurationManager.AppSettings["WebApiUrl"];
        }

        /// <summary>
        /// this action is the list of stories that current user has composed
        /// </summary>
        public async Task<ActionResult> Index()
        {
            try
            {
                List<Story> model = new List<Story>();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var resp = await client.GetAsync($"Story/Get");
                    model = await resp.Content.ReadAsAsync<List<Story>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

                model.ForEach(
                    x => {
                        x.Body = Regex.Replace(x.Body, "<.*?>", String.Empty);
                    });

                return View(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        #region Compose

        [HttpGet]
        public async Task<ActionResult> Compose()
        {
            try
            {
                await LoadSelectListItemsAsync();

                return View();
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        public async Task<ActionResult> StoryTags(Guid? storyId)
        {
            try
            {
                var tags = new List<SelectListItem>();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var respTag = await client.GetAsync($"Story/StoryTagGet");
                    var resultTag = await respTag.Content.ReadAsAsync<List<Tag>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });


                    Parallel.ForEach(resultTag,
                        item =>
                            tags.Add(new SelectListItem()
                            {
                                Value = item.Id.ToString(),
                                Text = item.Caption
                            })
                            );

                    //foreach (var item in resultTag.OrderBy(x => x.Caption))
                    //{
                    //    tags.Add(new SelectListItem()
                    //    {
                    //        Value = item.Id.ToString(),
                    //        Text = item.Caption
                    //    });
                    //}

                    if (storyId != null)
                    {
                        var respSelectedTag = await client.GetAsync($"Story/StoryTagGet?StoryId={storyId}");
                        List<Tag> selectedTags = await respSelectedTag.Content.ReadAsAsync<List<Tag>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

                        tags.Where(x => selectedTags.Select(y => y.Id.ToString()).Contains(x.Value)).ToList()
                            .ForEach(j => j.Selected = true);
                    }
                }

                return PartialView(tags);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                throw ex;
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Compose(StoryComposeViewModel entity)
        {
            try
            {
                Story model = new Story()
                {
                    Title = entity.Title,
                    Body = entity.Body,
                    TopicId = entity.TopicId,
                    StoryStatusId = entity.StoryStatusId,
                };

                if (!string.IsNullOrEmpty(entity.StoryCoverImgResized))
                {
                    string img = entity.StoryCoverImgResized.Substring("data:image/jpg;base64,".Length);
                    model.StoryImage = Convert.FromBase64String(img);
                }

                /* in "select2 tag mode" if a new tag is writed,
                   it creates a new option for select
                   and the text and value of it is the text written,
                   so here we check if the value is an id or text */
                if (entity.Tags != null)
                {
                    foreach (var item in entity.Tags)
                    {
                        Guid id;

                        if (!Guid.TryParse(item, out id))
                        {
                            id = Guid.Empty;
                        }

                        model.Tags.Add(new Tag()
                        {
                            Id = id,
                            Caption = item
                        });
                    }
                }

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var resp = await client.PostAsJsonAsync<Story>($"Story/Compose", model);
                }

                return RedirectToAction("Index", "Story");
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            try
            {
                await LoadSelectListItemsAsync();

                Story story = new Story();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var resp = await client.GetAsync($"Story/Get/{id}");
                    story = await resp.Content.ReadAsAsync<Story>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

                StoryEditViewModel model = new StoryEditViewModel()
                {
                    Id = story.Id,
                    UserProfileId = story.UserProfileId,
                    Title = story.Title,
                    Body = story.Body,
                    TopicId = story.TopicId,
                    StoryCoverImgResized = Convert.ToBase64String(story.StoryImage == null ? new byte[0] : story.StoryImage),
                    StoryStatusId = story.StoryStatusId,
                    ActionDate = story.ActionDate
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StoryEditViewModel entity)
        {
            try
            {
                Story model = new Story()
                {
                    Id = entity.Id,
                    UserProfileId = entity.UserProfileId,
                    Title = entity.Title,
                    Body = entity.Body,
                    TopicId = entity.TopicId,
                    StoryStatusId = entity.StoryStatusId,
                    ActionDate = entity.ActionDate
                };

                if (!string.IsNullOrEmpty(entity.StoryCoverImgResized))
                {
                    var b64Str = "data:image/png;base64,";

                    string img = entity.StoryCoverImgResized.StartsWith(b64Str) ? 
                            entity.StoryCoverImgResized.Substring(b64Str.Length) : entity.StoryCoverImgResized;
                    model.StoryImage = Convert.FromBase64String(img);
                }

                if (entity.Tags != null)
                {
                    foreach (var item in entity.Tags)
                    {
                        Guid id;

                        if (!Guid.TryParse(item, out id)) id = Guid.Empty;

                        model.Tags.Add(new Tag()
                        {
                            Id = id,
                            Caption = item
                        });
                    }
                }

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var resp = await client.PostAsJsonAsync<Story>($"Story/Edit", model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        #endregion

        #region View

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> View(Guid id)
        {
            try
            {
                Story model = new Story();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var resp = await client.GetAsync($"Story/Get/{id}");
                    model = await resp.Content.ReadAsAsync<Story>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        #endregion

        #region Delete

        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var resp = await client.GetAsync($"Story/Delete/{id}");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.HttpContext);
                return View("Error");
            }
        }

        #endregion

        #region Partial Actions

        public async Task<ActionResult> PartialIndex(Guid UserProfileId)
        {
            try
            {
                List<Story> model = new List<Story>();

                using (var client = new HttpClient(new AuthenticationHandler()))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var resp = await client.GetAsync($"Story/GetByUserProfileId/{UserProfileId}");
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

        #endregion


        #region Methods

        async Task LoadSelectListItemsAsync(Story entity = null)
        {
            var topics = new List<SelectListItem>();
            var statuses = new List<SelectListItem>();

            using (var client = new HttpClient(new AuthenticationHandler()))
            {
                client.BaseAddress = new Uri(baseAddress);

                #region Topics
                var respTopics = await client.GetAsync($"Topic/SelectedTopics");
                var resultTopics = await respTopics.Content.ReadAsAsync<List<Topic>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

                foreach (var item in resultTopics.OrderBy(x => x.Caption))
                {
                    topics.Add(new SelectListItem()
                    {
                        Value = item.Id.ToString(),
                        Text = item.Caption
                    });
                }

                if (entity != null)
                    topics.Single(x => x.Value == entity.TopicId.ToString()).Selected = true;
                #endregion

                #region Story Statuses
                var respStatus = await client.GetAsync($"Story/StoryStatusGet");
                var resultStatus = await respStatus.Content.ReadAsAsync<List<StoryStatus>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

                foreach (var item in resultStatus.Where(x => x.Value != StoryStatusValue.Deleted))
                {
                    statuses.Add(new SelectListItem()
                    {
                        Value = item.Id.ToString(),
                        Text = item.Caption,
                        Selected = item.Value == StoryStatusValue.Draft
                    });
                }

                if (entity != null)
                    statuses.Single(x => x.Value == entity.StoryStatusId.ToString()).Selected = true;
                #endregion
            }

            ViewBag.Topics = topics;
            ViewBag.StoryStatuses = statuses;
        }
        #endregion
    }
}