namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STR_CommentOfStory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public STR_CommentOfStory()
        {
            STR_CommentOfStory1 = new HashSet<STR_CommentOfStory>();
            STR_ReportOfComment = new HashSet<STR_ReportOfComment>();
        }

        public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }

        public Guid StoryId { get; set; }

        [Required]
        public string Comment { get; set; }

        public bool IsReply { get; set; }

        public Guid? ReplyToId { get; set; }

        public DateTime ActionDate { get; set; }

        public virtual PRF_UserProfile PRF_UserProfile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_CommentOfStory> STR_CommentOfStory1 { get; set; }

        public virtual STR_CommentOfStory STR_CommentOfStory2 { get; set; }

        public virtual STR_Story STR_Story { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_ReportOfComment> STR_ReportOfComment { get; set; }
    }
}
