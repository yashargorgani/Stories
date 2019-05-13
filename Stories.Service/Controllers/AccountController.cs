using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stories.Service.Models;
using System.Text;
using System.IO;
using System.Security.Principal;
using Newtonsoft.Json;
using Stories.Models;
using System.Configuration;
using System.Threading.Tasks;
using Stories.Service.Infrastructure;
using Stories.Service.Helpers;
using Stories.DataAccess;

namespace Stories.Service.Controllers
{
    public class AccountController : ApiController
    {
        string client_id;
        string client_secret;
        string returnUrl;
        string websiteUrl;
        string googleAuthUrl;
        string googleApisUrl;

        StoriesDbContext db;

        public AccountController(StoriesDbContext _db)
        {
            client_id = ConfigurationManager.AppSettings["GoogleClientId"];
            client_secret = ConfigurationManager.AppSettings["GoogleClientSecret"];
            returnUrl = ConfigurationManager.AppSettings["GoogleReturnUrl"];
            websiteUrl = ConfigurationManager.AppSettings["WebSiteUrl"];
            googleAuthUrl = "https://accounts.google.com/o/oauth2/";
            googleApisUrl = "https://www.googleapis.com/auth/";

            db = _db;
        }

        public IHttpActionResult GetWebLoginUri()
        {

            string url = $"{googleAuthUrl}auth?response_type=code&redirect_uri={returnUrl}&scope={googleApisUrl}userinfo.email%20{googleApisUrl}userinfo.profile&client_id={client_id}";

            return Ok(url);
        }

        [HttpGet]
        public async Task<IHttpActionResult> googlelogin(string code)
        {
            try
            {
                #region Get Access Token and Email

                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }

                GoogleAccessToken googleAccessToken = await GoogleAccessToken.GetTokenByCodeAsync(code);

                string accessToken = string.Empty;

                accessToken = googleAccessToken.access_token;

                if (string.IsNullOrEmpty(accessToken))
                {
                    return BadRequest();
                }

                GoogleUserOutputData userData = await GoogleUserOutputData.GetUserByTokenAsync(accessToken);

                var account = db.PRF_UserAccount.FirstOrDefault(x => x.EmailAddress == userData.email);

                bool isRegistered = account != null;
                
                //if the user account is not registered, will create an account
                if (!isRegistered)
                {
                    var userProfile = new DataAccess.Entities.PRF_UserProfile
                    {
                        Id = Guid.NewGuid(),
                        Name = userData.name,
                        RegisterDate = DateTime.Now
                    };

                    db.PRF_UserProfile.Add(userProfile);

                    db.PRF_UserAccount.Add(new DataAccess.Entities.PRF_UserAccount
                    {
                        Id = Guid.NewGuid(),
                        UserProfileId = userProfile.Id,
                        EmailAddress = userData.email
                    });

                    await db.SaveChangesAsync();
                }

                Request.GetRequestContext().Principal = 
                    new GenericPrincipal(new GenericIdentity(userData.email), new[] { "User" });

                string url = $"{websiteUrl}account/logedin?" +
                             $"access_token={googleAccessToken.access_token}&" +
                             $"email={userData.email}&" +
                             $"expires_in={googleAccessToken.expires_in}&" +
                             $"isRegistered={isRegistered}";

                return Redirect(url);
                
                #endregion
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            try
            {
                var entity = await db.PRF_UserAccount.FindAsync(Guid.Parse(User.Identity.Name));

                if (entity == null)
                    return NotFound();

                UserAccount model = entity.Map();

                return Ok(model);
            }
            catch (Exception ex)
            {
                ex.LogException(ControllerContext.RequestContext);
                throw ex;
            }
        }        
    }
}