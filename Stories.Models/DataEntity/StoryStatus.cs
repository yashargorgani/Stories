using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Models
{
    public class StoryStatusValue
    {
        public const string Draft = "Draft";
        public const string Publish = "Publish";
        public const string Deleted = "Deleted";
    }

    public class StoryStatus
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Value { get; set; }
    }
}
