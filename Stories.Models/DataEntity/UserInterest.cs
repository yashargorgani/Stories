namespace Stories.Models
{
    public class UserInterest
    {
        public System.Guid Id { get; set; }
        public int InterestId { get; set; }
        public System.Guid UserProfileId { get; set; }

        #region Linked Entities

        public string InterestCaption { get; set; }

        #endregion
    }
}