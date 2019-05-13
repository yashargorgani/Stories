using Stories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Dal.Interface
{
    public interface ITopicGateway : IGateway<Topic>
    {
        List<Topic> GetFavorites(int count);
    }
}
