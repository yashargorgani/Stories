using Stories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Dal.Interface
{
    public interface IStoryGateway : IGateway<Story>
    {
        List<Story> GetPaged(int page, Expression<Func<Story, bool>> filter = null);
    }
}
