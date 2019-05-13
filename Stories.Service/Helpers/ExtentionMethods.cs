using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Stories.DataAccess;

namespace Stories.Service.Helpers
{
    public static class ExtentionMethods
    {
        public static Guid? GetProfileId(this IIdentity identity)
        {
            using (var db = new StoriesDbContext())
            {
                var account = db.PRF_UserAccount.Find(Guid.Parse(identity.Name));

                if (account == null)
                    return null;

                else return account.UserProfileId;
            }
        }

        public static async Task<Guid?> GetProfileIdAsync(this IIdentity identity)
        {
            using (var db = new StoriesDbContext())
            {
                var account = await db.PRF_UserAccount.FindAsync(Guid.Parse(identity.Name));

                if (account == null)
                    return null;

                else return account.UserProfileId;
            }
        }
    }
}