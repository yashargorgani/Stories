namespace Stories.Models
{
    public class ReportOfComment
    {
        public System.Guid Id { get; set; }
        public System.Guid UserProfileId { get; set; }
        public System.Guid CommentId { get; set; }
        public int ReportSubjectId { get; set; }
        public System.DateTime ActionDate { get; set; }

        #region Linked Entities

        public string ReportSubjectCaption { get; set; }

        #endregion

    }
}