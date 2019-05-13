namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STR_Story
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public STR_Story()
        {
            STR_CommentOfStory = new HashSet<STR_CommentOfStory>();
            STR_LikeOfStory = new HashSet<STR_LikeOfStory>();
            STR_RateOfStory = new HashSet<STR_RateOfStory>();
            STR_ReportOfStory = new HashSet<STR_ReportOfStory>();
            STR_TagOfStory = new HashSet<STR_TagOfStory>();
        }

        public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public int TopicId { get; set; }

        public byte[] StoryImage { get; set; }

        public int StoryStatusId { get; set; }

        public DateTime ActionDate { get; set; }

        public virtual PRF_UserProfile PRF_UserProfile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_CommentOfStory> STR_CommentOfStory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_LikeOfStory> STR_LikeOfStory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_RateOfStory> STR_RateOfStory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_ReportOfStory> STR_ReportOfStory { get; set; }

        public virtual STR_Topic STR_Topic { get; set; }

        public virtual STR_StoryStatus STR_StoryStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_TagOfStory> STR_TagOfStory { get; set; }
    }
}
