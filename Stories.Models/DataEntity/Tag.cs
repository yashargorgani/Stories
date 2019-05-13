using System.Collections.Generic;

namespace Stories.Models
{
    public class Tag
    {
        public System.Guid Id { get; set; }
        public string Caption { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}