using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Stories.Models
{
    public class Story
    {
        public Story()
        {
            Tags = new List<Tag>();
        }

        public System.Guid Id { get; set; }
        public System.Guid UserProfileId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int TopicId { get; set; }
        public byte[] StoryImage { get; set; }
        public int StoryStatusId { get; set; }
        public System.DateTime ActionDate { get; set; }

        #region Linked Entities

        public string StoryStatusCaption { get; set; }
        public string StoryStatusValue { get; set; }
        public string TopicCaption { get; set; }
        public List<Tag> Tags { get; set; }
        public string UserProfileName { get; set; }
        public byte[] UserProfileImage { get; set; }

        #endregion


    }
}