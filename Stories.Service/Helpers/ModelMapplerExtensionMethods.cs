using Stories.DataAccess.Entities;
using Stories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stories.Service.Helpers
{
    public static class ModelMapplerExtensionMethods
    {
        public static UserAccount Map(this PRF_UserAccount entity)
        {
            return new UserAccount
            {
                Id = entity.Id,
                UserProfileId = entity.UserProfileId,
                EmailAddress = entity.EmailAddress,
                Token = entity.Token
            };
        }

        public static PRF_UserAccount Map(this UserAccount entity)
        {
            return new PRF_UserAccount()
            {
                Id = entity.Id,
                UserProfileId = entity.UserProfileId,
                EmailAddress = entity.EmailAddress,
                Token = entity.Token
            };
        }

        public static Story Map(this STR_Story entity)
        {
            return new Story
            {
                Id = entity.Id,
                UserProfileId = entity.UserProfileId,
                Title = entity.Title,
                Body = entity.Body,
                TopicId = entity.TopicId,
                StoryImage = entity.StoryImage,
                ActionDate = entity.ActionDate,
                StoryStatusId = entity.StoryStatusId,

                StoryStatusCaption = entity.STR_StoryStatus.Caption,
                StoryStatusValue = entity.STR_StoryStatus.Value,
                TopicCaption = entity.STR_Topic.Caption,
                UserProfileName = entity.PRF_UserProfile.Name,
                UserProfileImage = entity.PRF_UserProfile.ProfileImage
            };
        }

        public static STR_Story Map(this Story entity)
        {
            return new STR_Story
            {
                Id = entity.Id,
                UserProfileId = entity.UserProfileId,
                Title = entity.Title,
                Body = entity.Body,
                TopicId = entity.TopicId,
                StoryImage = entity.StoryImage,
                StoryStatusId = entity.StoryStatusId,
                ActionDate = entity.ActionDate,
            };
        }

        public static Tag Map(this STR_Tag entity)
        {
            return new Tag
            {
                Id = entity.Id,
                Caption = entity.Caption,
                CreateDate = entity.CreateDate
            };
        }

        public static StoryStatus Map(this STR_StoryStatus entity)
        {
            return new StoryStatus()
            {
                Id = entity.Id,
                Caption = entity.Caption,
                Value = entity.Value
            };
        }

        public static Topic Map(this STR_Topic entity)
        {
            return new Topic
            {
                Id = entity.Id,
                Caption = entity
.Caption
            };
        }

        public static UserProfile Map(this PRF_UserProfile entity)
        {
            return new UserProfile
            {
                Id = entity.Id,
                Name = entity.Name,
                Bio = entity.Bio,
                MobileNumber = entity.MobileNumber,
                ProfileImage = entity.ProfileImage,
                RegisterDate = entity.RegisterDate
            };
        }

        public static PRF_UserProfile Map(this UserProfile entity)
        {
            return new PRF_UserProfile
            {
                Id = entity.Id,
                Name = entity.Name,
                Bio = entity.Bio,
                MobileNumber = entity.MobileNumber,
                ProfileImage = entity.ProfileImage,
                RegisterDate = entity.RegisterDate
            };
        }
    }
}