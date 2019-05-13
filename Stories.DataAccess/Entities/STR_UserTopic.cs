namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STR_UserTopic
    {
        public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }

        public int TopicId { get; set; }

        public virtual STR_Topic STR_Topic { get; set; }
    }
}
