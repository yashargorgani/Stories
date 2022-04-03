using Stories.DataAccess;
using Stories.DataAccess.Entities;
using Stories.Helpers;
using Stories.Models;
using Stories.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Stories.Service.Controllers
{
    [Authorize]
    public class StoryController : ApiController
    {
        StoriesDbContext db;
        //IQueryable<STR_Story> query;

        public StoryController(StoriesDbContext _db)
        {
            db = _db;
            //query = db.STR_Story.Where(x => x.STR_StoryStatus.Value == StoryStatusValue.Publish);
        }        

        #region GET STORY

        /// <summary>
        /// get a story by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {               
                var entity = db.STR_Story.Find(id);

                if (entity == null)
                    return NotFound();

                var model = entity.Map();

                model.Tags =
                    (await db.STR_TagOfStory
                    .Where(x => x.StoryId == id)
                    .Select(x => x.STR_Tag).ToListAsync()).Select(x => x.Map()).ToList();

                model.Rates = entity.STR_RateOfStory.Select(x => x.Map()).ToList();

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        /// <summary>
        /// get stories of CURRENT user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var profileId = await User.Identity.GetProfileIdAsync();

                var model =
                    db.STR_Story
                    .Where(x => x.UserProfileId == profileId && 
                    x.STR_StoryStatus.Value != StoryStatusValue.Deleted)
                    .ToList().Select(x => x.Map());

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        /// <summary>
        /// get list of PUBLISHED stories of a User Profile Id
        /// </summary>
        /// <param name="id">User Profile Id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetByUserProfileId(Guid id)
        {
            try
            {
                var model =
                    (await db.STR_Story
                    .Where(x => x.UserProfileId == id && x.STR_StoryStatus.Value == StoryStatusValue.Publish)
                    .ToListAsync()).Select(x => x.Map());

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetForHomePage([FromUri]SearchParam sp)
        {
            try
            {
                var count = 10;
                var stories = new List<Story>();
                IQueryable<STR_Story> query = db.STR_Story.Where(x => x.STR_StoryStatus.Value == StoryStatusValue.Publish);

                if (!string.IsNullOrEmpty(sp.q))
                {
                    query = query.Where(x =>
                        x.Title.Contains(sp.q) ||
                        x.STR_Topic.Caption.Contains(sp.q) ||
                        x.PRF_UserProfile.Name.Contains(sp.q) ||
                        x.STR_TagOfStory.Select(t => t.STR_Tag.Caption).Contains(sp.q));
                }

                if (sp.topic.HasValue)
                {
                    query = query.Where(x =>
                        x.TopicId == sp.topic.Value);
                }

                if (sp.tag.HasValue)
                {
                    query = query.Where(x =>
                        x.STR_TagOfStory.Any(t => t.TagId == sp.tag.Value));
                }

                if (User.Identity.IsAuthenticated)
                {
                    var profileId = await User.Identity.GetProfileIdAsync();

                    var topics = 
                        db.STR_UserTopic
                        .Where(x => x.UserProfileId == profileId)
                        .Select(x => x.TopicId);

                    query = query.Where(x => topics.Contains(x.TopicId));
                }

                stories =
                    (await query.OrderByDescending(x => x.ActionDate)
                    .Skip((sp.page - 1) * count).Take(count).ToListAsync()).Select(x => x.Map()).ToList();

                Parallel.ForEach(stories.Where(x => x.StoryImage != null),
                item =>
                {
                    item.StoryImage = item.StoryImage.ReduceSize();
                });

                return Ok(stories);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        #endregion

        #region CRUD

        [HttpPost]
        public async Task<IHttpActionResult> Compose(Story item)
        {
            try
            {
                STR_Story entity = item.Map();

                entity.Id = Guid.NewGuid();
                entity.UserProfileId = (await User.Identity.GetProfileIdAsync()).Value;
                entity.ActionDate = DateTime.Now;

                db.STR_Story.Add(entity);

                foreach (var tag in item.Tags)
                {
                    if (tag.Id == Guid.Empty)
                    {
                        tag.Id = Guid.NewGuid();

                        db.STR_Tag.Add(new STR_Tag
                        {
                            Id = tag.Id,
                            Caption = tag.Caption,
                            CreateDate = DateTime.Now
                        });
                    }

                    db.STR_TagOfStory.Add(new STR_TagOfStory
                    {
                        Id = Guid.NewGuid(),
                        StoryId = entity.Id,
                        TagId = tag.Id
                    });
                }

                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Edit(Story item)
        {
            try
            {
                var profileId = await User.Identity.GetProfileIdAsync();

                if (!profileId.HasValue || 
                    item.UserProfileId != profileId.Value)
                {
                    return Ok("NotOwner");
                }

                var status = db.STR_StoryStatus.First(x => x.Id == item.StoryStatusId);

                //action date is the date of story publish
                if (status.Value == StoryStatusValue.Publish)
                    item.ActionDate = DateTime.Now;

                STR_Story entity = item.Map();

                db.Entry(entity).State = EntityState.Modified;

                //first we delete all tags of this story from db
                db.STR_TagOfStory.Where(x => x.StoryId == item.Id).ToList()
                    .ForEach(x => db.STR_TagOfStory.Remove(x));

                //then add all tags
                foreach (var tag in item.Tags)
                {
                    if (tag.Id == Guid.Empty)
                    {
                        tag.Id = Guid.NewGuid();

                        db.STR_Tag.Add(new STR_Tag
                        {
                            Id = tag.Id,
                            Caption = tag.Caption,
                            CreateDate = DateTime.Now
                        });
                    }

                    db.STR_TagOfStory.Add(new STR_TagOfStory
                    {
                        Id = Guid.NewGuid(),
                        StoryId = entity.Id,
                        TagId = tag.Id
                    });
                }

                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                var story = await db.STR_Story.FindAsync(id);
                var profileId = await User.Identity.GetProfileIdAsync();

                if (!profileId.HasValue ||
                   story.UserProfileId != profileId.Value)
                {
                    return Ok("NotOwner");
                }

                var delState = db.STR_StoryStatus.First(x => x.Value == StoryStatusValue.Deleted);

                story.StoryStatusId = delState.Id;
                db.Entry(story).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        #endregion

        #region Story Tag

        [HttpGet]
        public async Task<IHttpActionResult> StoryTagGet()
        {
            try
            {
                var model = (await db.STR_Tag.ToListAsync()).Select(x => x.Map());

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> StoryTagGet(Guid StoryId)
        {
            try
            {
                var tagIds =
                    await db.STR_TagOfStory
                    .Where(x => x.StoryId == StoryId)
                    .Select(x => x.TagId).ToListAsync();
                var model =
                    (await db.STR_Tag
                    .Where(x => tagIds.Contains(x.Id)).ToListAsync())
                    .Select(x => x.Map());

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        #endregion

        [HttpGet]
        public async Task<IHttpActionResult> StoryStatusGet()
        {
            try
            {
                var model = 
                    (await db.STR_StoryStatus.ToListAsync()).Select(x => x.Map());

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RateSubjects()
        {
            try
            {
                var model = await db.STR_RateSubject
                        .Select(x => new RateSubject
                        {
                            Id = x.Id,
                            Caption = x.Caption
                        }).ToListAsync();

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Rate(RateStory rate)
        {
            try
            {
                var userId = User.Identity.GetProfileId().Value;

                if (rate.Rate > 5 || rate.Rate < 1)
                {
                    return BadRequest("Rate should be between one and five.");
                }

                var entity = await db.STR_RateOfStory
                    .FirstOrDefaultAsync(x =>
                    x.UserProfileId ==  userId &&
                    x.StoryId == rate.StoryId && x.RateSubjectId == rate.RateSubjectId);

                if(entity != null)
                {
                    entity.Rate = rate.Rate;

                    db.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    entity = new STR_RateOfStory
                    {
                        StoryId = rate.StoryId,
                        UserProfileId = userId,
                        RateSubjectId = rate.RateSubjectId,
                        Rate = rate.Rate,
                    };

                    db.STR_RateOfStory.Add(entity);
                }                

                await db.SaveChangesAsync();

                return Ok(entity.Map());
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                return BadRequest(ex.Message);
            }            
        }
    }
}
