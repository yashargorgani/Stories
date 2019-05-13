namespace Stories.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Stories.DataAccess.Entities;

    public partial class StoriesDbContext : DbContext
    {
        public StoriesDbContext()
            : base("name=StoriesDbContext")
        {
        }

        public virtual DbSet<PRF_EducationField> PRF_EducationField { get; set; }
        public virtual DbSet<PRF_EducationLevel> PRF_EducationLevel { get; set; }
        public virtual DbSet<PRF_Follow> PRF_Follow { get; set; }
        public virtual DbSet<PRF_Interest> PRF_Interest { get; set; }
        public virtual DbSet<PRF_UserAccount> PRF_UserAccount { get; set; }
        public virtual DbSet<PRF_UserInterest> PRF_UserInterest { get; set; }
        public virtual DbSet<PRF_UserProfile> PRF_UserProfile { get; set; }
        public virtual DbSet<PRF_WorkField> PRF_WorkField { get; set; }
        public virtual DbSet<STR_CommentOfStory> STR_CommentOfStory { get; set; }
        public virtual DbSet<STR_LikeOfStory> STR_LikeOfStory { get; set; }
        public virtual DbSet<STR_RateOfStory> STR_RateOfStory { get; set; }
        public virtual DbSet<STR_RateSubject> STR_RateSubject { get; set; }
        public virtual DbSet<STR_ReportOfComment> STR_ReportOfComment { get; set; }
        public virtual DbSet<STR_ReportOfStory> STR_ReportOfStory { get; set; }
        public virtual DbSet<STR_ReportSubject> STR_ReportSubject { get; set; }
        public virtual DbSet<STR_Story> STR_Story { get; set; }
        public virtual DbSet<STR_StoryStatus> STR_StoryStatus { get; set; }
        public virtual DbSet<STR_Tag> STR_Tag { get; set; }
        public virtual DbSet<STR_TagOfStory> STR_TagOfStory { get; set; }
        public virtual DbSet<STR_Topic> STR_Topic { get; set; }
        public virtual DbSet<STR_UserTopic> STR_UserTopic { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PRF_EducationField>()
                .HasMany(e => e.PRF_UserProfile)
                .WithOptional(e => e.PRF_EducationField)
                .HasForeignKey(e => e.EducationFieldId);

            modelBuilder.Entity<PRF_EducationLevel>()
                .HasMany(e => e.PRF_UserProfile)
                .WithOptional(e => e.PRF_EducationLevel)
                .HasForeignKey(e => e.EducationLevelId);

            modelBuilder.Entity<PRF_Interest>()
                .HasMany(e => e.PRF_UserInterest)
                .WithRequired(e => e.PRF_Interest)
                .HasForeignKey(e => e.InterestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserAccount>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.PRF_Follow)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.FollowingUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.PRF_Follow1)
                .WithRequired(e => e.PRF_UserProfile1)
                .HasForeignKey(e => e.FollowedUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.PRF_UserAccount)
                .WithOptional(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.PRF_UserInterest)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.STR_CommentOfStory)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.STR_RateOfStory)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.STR_ReportOfComment)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.STR_ReportOfStory)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.STR_Story)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_UserProfile>()
                .HasMany(e => e.STR_LikeOfStory)
                .WithRequired(e => e.PRF_UserProfile)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRF_WorkField>()
                .HasMany(e => e.PRF_UserProfile)
                .WithOptional(e => e.PRF_WorkField)
                .HasForeignKey(e => e.WorkFieldId);

            modelBuilder.Entity<STR_CommentOfStory>()
                .HasMany(e => e.STR_CommentOfStory1)
                .WithOptional(e => e.STR_CommentOfStory2)
                .HasForeignKey(e => e.ReplyToId);

            modelBuilder.Entity<STR_CommentOfStory>()
                .HasMany(e => e.STR_ReportOfComment)
                .WithRequired(e => e.STR_CommentOfStory)
                .HasForeignKey(e => e.CommentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_RateSubject>()
                .HasMany(e => e.STR_RateOfStory)
                .WithRequired(e => e.STR_RateSubject)
                .HasForeignKey(e => e.RateSubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_ReportSubject>()
                .HasMany(e => e.STR_ReportOfComment)
                .WithRequired(e => e.STR_ReportSubject)
                .HasForeignKey(e => e.ReportSubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_ReportSubject>()
                .HasMany(e => e.STR_ReportOfStory)
                .WithRequired(e => e.STR_ReportSubject)
                .HasForeignKey(e => e.ReportSubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Story>()
                .HasMany(e => e.STR_CommentOfStory)
                .WithRequired(e => e.STR_Story)
                .HasForeignKey(e => e.StoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Story>()
                .HasMany(e => e.STR_LikeOfStory)
                .WithRequired(e => e.STR_Story)
                .HasForeignKey(e => e.StoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Story>()
                .HasMany(e => e.STR_RateOfStory)
                .WithRequired(e => e.STR_Story)
                .HasForeignKey(e => e.StoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Story>()
                .HasMany(e => e.STR_ReportOfStory)
                .WithRequired(e => e.STR_Story)
                .HasForeignKey(e => e.StoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Story>()
                .HasMany(e => e.STR_TagOfStory)
                .WithRequired(e => e.STR_Story)
                .HasForeignKey(e => e.StoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_StoryStatus>()
                .Property(e => e.Value)
                .IsUnicode(false);

            modelBuilder.Entity<STR_StoryStatus>()
                .HasMany(e => e.STR_Story)
                .WithRequired(e => e.STR_StoryStatus)
                .HasForeignKey(e => e.StoryStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Tag>()
                .HasMany(e => e.STR_TagOfStory)
                .WithRequired(e => e.STR_Tag)
                .HasForeignKey(e => e.TagId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Topic>()
                .HasMany(e => e.STR_Story)
                .WithRequired(e => e.STR_Topic)
                .HasForeignKey(e => e.TopicId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STR_Topic>()
                .HasMany(e => e.STR_UserTopic)
                .WithRequired(e => e.STR_Topic)
                .HasForeignKey(e => e.TopicId)
                .WillCascadeOnDelete(false);
        }
    }
}
