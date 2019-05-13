namespace Stories.Models
{
    public class ReportOfStory
    {
        public System.Guid Id { get; set; }
        public System.Guid UserProfileId { get; set; }
        public System.Guid StoryId { get; set; }
        public int ReportSubjectId { get; set; }
        public System.DateTime ActionDate { get; set; }

        #region List Entities

        public string ReportSubjectCaption { get; set; }
        public string StoryTitle { get; set; }

        #endregion
    }
}