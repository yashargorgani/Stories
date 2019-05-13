using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Stories.Models;
using Stories.Service.Helpers;
using Stories.DataAccess;
using Stories.DataAccess.Entities;
using System.Data.Entity;
//using Stories.Dal.DataEntity;

namespace Stories.Service.Controllers
{
    [Authorize]
    public class UserProfileController : ApiController
    {
        StoriesDbContext db;

        public UserProfileController(StoriesDbContext _db)
        {
            db = _db;
        }

        /// <summary>
        /// get the user profile by id
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                var entity = await db.PRF_UserProfile.FindAsync(id);

                if (entity == null)
                    return NotFound();

                var userProfile = entity.Map();

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        /// <summary>
        /// get the current user profile
        /// </summary>
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var entity =
                    (await db.PRF_UserAccount
                    .FindAsync(Guid.Parse(User.Identity.Name))).PRF_UserProfile;

                if (entity == null)
                    return NotFound();

                UserProfile profile = entity.Map();

                return Ok(profile);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Create()
        {
            try
            {
                var entity =
                    (await db.PRF_UserAccount
                    .FindAsync(Guid.Parse(User.Identity.Name))).PRF_UserProfile;

                if (entity == null)
                    return NotFound();

                UserProfile profile = entity.Map();

                return Ok(profile);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(UserProfile entity)
        {
            try
            {
                PRF_UserProfile item = entity.Map();

                db.Entry(item).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }
    }
}
