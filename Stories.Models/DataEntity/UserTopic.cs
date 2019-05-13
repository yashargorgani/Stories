using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Models
{
    public class UserTopic
    {
        public System.Guid Id { get; set; }
        public System.Guid UserProfileId { get; set; }
        public int TopicId { get; set; }

        #region Linked Entities

        public string TopicCaption { get; set; }

        #endregion
    }
}
