using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Models
{
    public class Follow
    {
        public System.Guid Id { get; set; }
        public System.Guid FollowingUserId { get; set; }
        public System.Guid FollowedUserId { get; set; }
        public System.DateTime ActionDate { get; set; }

   }
}
