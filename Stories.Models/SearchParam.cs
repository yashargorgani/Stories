using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Models
{
    public class SearchParam
    {
        private int _page;
        public int page
        {
            get { return _page; }
            set { _page = value > 0 ? value : 1; }
        }
        public Guid? tag { get; set; }
        public int? topic { get; set; }
        public string q { get; set; }

        public string ToUrl()
        {
            string url = string.Empty;


            url += "&page=" + (page > 0 ? page : 1);

            if(tag.HasValue)
                url = url + "&tag=" + tag.Value;

            if(topic.HasValue)
                url = url + "&topic=" + topic.Value;

            if(!string.IsNullOrEmpty(q))
                url = url + "&q=" + q;

            if(url.StartsWith("&"))
                url = url.Substring(1);

            if(!string.IsNullOrEmpty(url))
                url = "?" + url;

            return url;
        }
    }
}
