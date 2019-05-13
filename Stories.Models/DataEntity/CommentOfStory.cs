using System;
using System.Collections.Generic;

namespace Stories.Models
{
    public class CommentOfStory
    {
        public System.Guid Id { get; set; }
        public System.Guid UserProfileId { get; set; }
        public System.Guid StoryId { get; set; }
        public string Comment { get; set; }
        public bool IsReply { get; set; }
        public Guid? ReplyToId { get; set; }
        public System.DateTime ActionDate { get; set; }

        #region Linked Entities

        #endregion

    }
}