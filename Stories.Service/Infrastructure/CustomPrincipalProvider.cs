using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Stories.Service.Models;
using Stories.DataAccess;
using Stories.DataAccess.Entities;

namespace Stories.Service.Infrastructure
{
    public class CustomPrincipalProvider
    {
        public async Task<IPrincipal> CreatePrincipals(string email, string accessToken)
        {
            GoogleUserOutputData user = await GoogleUserOutputData.GetUserByTokenAsync(accessToken);

            PRF_UserAccount userAccount = null; 

            using(var db = new StoriesDbContext())
            {
                userAccount = db.PRF_UserAccount.FirstOrDefault(x => x.EmailAddress == email);
            }
            
            //the case the google account is not correct
            if (user == null || user.email != email || userAccount == null)
            {
                return 
                    new GenericPrincipal(new GenericIdentity(""), new string[0]);
            }

            var identity = new GenericIdentity(userAccount.Id.ToString());

            IPrincipal principal = new GenericPrincipal(identity, new[] { "User" });

            return principal;
        }
    }
}