using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stories.Models;
using Stories.Dal.DataEntity;
using System.Linq.Expressions;
using Stories.Dal.Interface;

namespace Stories.Dal.Gateway
{
    public class InterestGateway : IInterestGateway
    {
        private StoriesEntities db;

        public InterestGateway()
        {
            db = new StoriesEntities();
        }

        public UnitOfWork Unit
        {
            get { return _Unit; }
            set
            {
                _Unit = value;
                SetUnitOfWork(_Unit);
            }
        }

        private UnitOfWork _Unit;

        protected void SetUnitOfWork(UnitOfWork unit)
        {
            if (unit != null)
            {
                if (unit.dbContext == null)
                    unit.dbContext = db;
                else
                    this.db = unit.dbContext;
            }
        }

        private IQueryable<Interest> GetQuery()
        {
            return db.PRF_Interest.Select(x =>
                new Interest()
                {
                    Id = x.Id,
                    Caption = x.Caption
                });
        }

        private PRF_Interest GetMapped(Interest x)
        {
            return new PRF_Interest()
            {
                Id = x.Id,
                Caption = x.Caption
            };
        }

        public List<Interest> Get(Expression<Func<Interest, bool>> filter = null)
        {
            try
            {
                var query = GetQuery();

                return query.Where(filter ?? (x => true)).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Interest SingleOrNull(Expression<Func<Interest, bool>> filter = null)
        {
            try
            {
                return Get(filter).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Add(Interest item)
        {
            try
            {
                PRF_Interest entity = GetMapped(item);

                db.PRF_Interest.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(Interest item)
        {
            try
            {
                PRF_Interest entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(Interest item)
        {
            try
            {
                PRF_Interest entity = db.PRF_Interest.Find(item.Id);

                if (entity != null)
                    db.PRF_Interest.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<Interest, bool>> filter = null)
        {
            try
            {
                var query = GetQuery();

                return query.Count(filter ?? (x => true));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
