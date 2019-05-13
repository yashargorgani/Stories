namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STR_ReportOfStory
    {
        public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }

        public Guid StoryId { get; set; }

        public int ReportSubjectId { get; set; }

        public DateTime ActionDate { get; set; }

        public virtual PRF_UserProfile PRF_UserProfile { get; set; }

        public virtual STR_ReportSubject STR_ReportSubject { get; set; }

        public virtual STR_Story STR_Story { get; set; }
    }
}
