namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRF_UserInterest
    {
        public Guid Id { get; set; }

        public int InterestId { get; set; }

        public Guid UserProfileId { get; set; }

        public virtual PRF_Interest PRF_Interest { get; set; }

        public virtual PRF_UserProfile PRF_UserProfile { get; set; }
    }
}
