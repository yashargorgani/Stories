namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRF_UserAccount
    {
        public Guid Id { get; set; }

        public Guid? UserProfileId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [StringLength(256)]
        public string Token { get; set; }

        public virtual PRF_UserProfile PRF_UserProfile { get; set; }
    }
}
