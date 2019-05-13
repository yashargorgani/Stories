using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Stories.DataAccess;
using Stories.DataAccess.Entities;
using Stories.Models;
using Stories.Service.Helpers;

namespace Stories.Service.Controllers
{
    [Authorize]
    public class TopicController : ApiController
    {
        private readonly StoriesDbContext db;

        public TopicController(StoriesDbContext _db)
        {
            db = _db;
        }

        /// <summary>
        /// Get list of topics selected by a user
        /// if UserProfileId is null, it returns current user's selected topics
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> SelectedTopics(Guid? UserProfileId = null)
        {
            try
            {
                if (UserProfileId == null)
                {
                    UserProfileId = await User.Identity.GetProfileIdAsync();
                }

                List<int> ids =
                    await db.STR_UserTopic
                    .Where(x => x.UserProfileId == UserProfileId)
                    .Select(x => x.TopicId).ToListAsync();

                var model =
                    (await db.STR_Topic.Where(x => ids.Contains(x.Id)).ToListAsync()).Select(x => x.Map());

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        /// <summary>
        /// list of all topics in database
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> AllTopics()
        {
            try
            {
                var model =
                    (await db.STR_Topic.ToListAsync()).Select(x => x.Map());

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        /// <summary>
        /// save topics selected by a user
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> Select(List<int> entity)
        {
            try
            {
                var profileId = await User.Identity.GetProfileIdAsync();

                var selectedTopics =
                    await db.STR_UserTopic
                    .Where(x => x.UserProfileId == profileId).ToListAsync();

                selectedTopics
                    .ForEach(x => db.STR_UserTopic.Remove(x));

                foreach (var id in entity)
                {
                    db.STR_UserTopic.Add(new STR_UserTopic
                    {
                        Id = Guid.NewGuid(),
                        UserProfileId = profileId.Value,
                        TopicId = id
                    });
                }

                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                return new ExceptionResult(ex, this);
            }
        }

        /// <summary>
        /// The list of topics to show on header
        /// If user has registered the result is the list of topics that user selected
        /// else its top favorite 15 topics
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> TopicsForHeader()
        {
            try
            {
                var topics = new List<Topic>();

                if (User.Identity.IsAuthenticated)
                {
                    var profileId = await User.Identity.GetProfileIdAsync();

                    var ids =
                        await db.STR_UserTopic.Where(x => x.UserProfileId == profileId).Select(x => x.TopicId).ToListAsync();

                    topics =
                        await db.STR_Topic.Where(x => ids.Contains(x.Id)).Select(x =>
                        new Topic
                        {
                            Id = x.Id,
                            Caption = x.Caption
                        }).ToListAsync();
                }
                else
                {
                    var favs =
                    db.STR_Topic
                        .Select(x => new { Id = x.Id, Count = x.STR_UserTopic.Count() })
                            .OrderBy(x => x.Count).Take(10).Select(x => x.Id);

                    topics =
                        await db.STR_Topic.Where(x => favs.Contains(x.Id)).Select(x =>
                            new Topic()
                            {
                                Id = x.Id,
                                Caption = x.Caption
                            }).ToListAsync();
                }

                return Ok(topics);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }
    }
}
