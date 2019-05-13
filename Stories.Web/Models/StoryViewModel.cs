using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stories.Web.Models
{
    public class StoryComposeViewModel
    {
        public StoryComposeViewModel()
        {
            Tags = new List<string>();
        }

        [Required(ErrorMessage = "لطفا وارد نمایید")]
        public string Title { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "لطفا وارد نمایید")]
        public string Body { get; set; }

        [Required(ErrorMessage = "لطفا وارد نمایید")]
        public int TopicId { get; set; }

        public string StoryCoverImgResized { get; set; }

        [Required(ErrorMessage = "لطفا وارد نمایید")]
        public int StoryStatusId { get; set; }
        
        public List<string> Tags { get; set; }

    }

    public class StoryEditViewModel : StoryComposeViewModel
    {
        public StoryEditViewModel() : base()
        {

        }

        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }
        public DateTime ActionDate { get; set; }
    }
}