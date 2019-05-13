namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRF_Follow
    {
        public Guid Id { get; set; }

        public Guid FollowingUserId { get; set; }

        public Guid FollowedUserId { get; set; }

        public DateTime ActionDate { get; set; }

        public virtual PRF_UserProfile PRF_UserProfile { get; set; }

        public virtual PRF_UserProfile PRF_UserProfile1 { get; set; }
    }
}
