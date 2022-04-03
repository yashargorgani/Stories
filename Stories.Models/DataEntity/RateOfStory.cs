namespace Stories.Models
{
    public class RateOfStory
    {
        public System.Guid Id { get; set; }
        public System.Guid UserProfileId { get; set; }
        public System.Guid StoryId { get; set; }
        public int RateSubjectId { get; set; }
        public short Rate { get; set; }
        public System.DateTime ActionDate { get; set; }


        #region Linked Entities

        public string RateSubjectCaption { get; set; }

        #endregion

    }

    public class RateStory
    {        
        public int RateSubjectId { get; set; }
        public System.Guid StoryId { get; set; }
        public short Rate { get; set; }
    }
}