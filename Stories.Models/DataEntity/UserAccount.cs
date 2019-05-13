using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Models
{
    public class UserAccount
    {
        public System.Guid Id { get; set; }
        public System.Guid? UserProfileId { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }

        #region Linked Entity



        #endregion

    }
}
