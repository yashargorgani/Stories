using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Dal
{
    public interface IGateway
    {
        UnitOfWork Unit { get; set; }
    }

    public interface IGateway<T> : IGateway
    {
        int Count(Expression<Func<T, bool>> filter = null);

        List<T> Get(Expression<Func<T, bool>> filter = null);

        T SingleOrNull(Expression<Func<T, bool>> filter = null);

        void Update(T item);

        void Add(T item);

        void Remove(T item);
    }
}
