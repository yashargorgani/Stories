namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STR_TagOfStory
    {
        public Guid Id { get; set; }

        public Guid StoryId { get; set; }

        public Guid TagId { get; set; }

        public virtual STR_Story STR_Story { get; set; }

        public virtual STR_Tag STR_Tag { get; set; }
    }
}
