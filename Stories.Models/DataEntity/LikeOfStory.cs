using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Models
{
    public class LikeOfStory
    {
        public System.Guid Id { get; set; }
        public System.Guid StoryId { get; set; }
        public System.Guid UserProfileId { get; set; }
        public bool Value { get; set; }
        public System.DateTime ActionDate { get; set; }
    }
}
