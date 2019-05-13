namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRF_UserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRF_UserProfile()
        {
            PRF_Follow = new HashSet<PRF_Follow>();
            PRF_Follow1 = new HashSet<PRF_Follow>();
            PRF_UserAccount = new HashSet<PRF_UserAccount>();
            PRF_UserInterest = new HashSet<PRF_UserInterest>();
            STR_CommentOfStory = new HashSet<STR_CommentOfStory>();
            STR_RateOfStory = new HashSet<STR_RateOfStory>();
            STR_ReportOfComment = new HashSet<STR_ReportOfComment>();
            STR_ReportOfStory = new HashSet<STR_ReportOfStory>();
            STR_Story = new HashSet<STR_Story>();
            STR_LikeOfStory = new HashSet<STR_LikeOfStory>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(12)]
        public string MobileNumber { get; set; }

        public int? EducationLevelId { get; set; }

        public int? EducationFieldId { get; set; }

        public int? WorkFieldId { get; set; }

        public string Bio { get; set; }

        public byte[] ProfileImage { get; set; }

        public DateTime RegisterDate { get; set; }

        public virtual PRF_EducationField PRF_EducationField { get; set; }

        public virtual PRF_EducationLevel PRF_EducationLevel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRF_Follow> PRF_Follow { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRF_Follow> PRF_Follow1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRF_UserAccount> PRF_UserAccount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRF_UserInterest> PRF_UserInterest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_CommentOfStory> STR_CommentOfStory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_RateOfStory> STR_RateOfStory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_ReportOfComment> STR_ReportOfComment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_ReportOfStory> STR_ReportOfStory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_Story> STR_Story { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_LikeOfStory> STR_LikeOfStory { get; set; }

        public virtual PRF_WorkField PRF_WorkField { get; set; }
    }
}
