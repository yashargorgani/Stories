//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Stories.Dal.DataEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class STR_UserTopic
    {
        public System.Guid Id { get; set; }
        public System.Guid UserProfileId { get; set; }
        public int TopicId { get; set; }
    
        public virtual STR_Topic STR_Topic { get; set; }
    }
}
