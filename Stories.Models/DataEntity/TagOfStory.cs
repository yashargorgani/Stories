using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Stories.Models
{
    public class TagOfStory
    {
       [DisplayName("")]
        public System.Guid Id { get; set; }
        public System.Guid StoryId { get; set; }
        public System.Guid TagId { get; set; }


        #region Linked Entities

        public string TagCaption { get; set; }

        #endregion

    }
}