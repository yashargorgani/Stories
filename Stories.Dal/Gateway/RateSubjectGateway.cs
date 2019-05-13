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
    public class RateSubjectGateway : IRateSubjectGateway
    {
        private StoriesEntities db;

        public RateSubjectGateway()
        {
            db = new StoriesEntities();
        }

        public UnitOfWork Unit
        {
            get
            {
                return _Unit;
            }
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

        private IQueryable<RateSubject> GetQuery()
        {
            return db.STR_RateSubject.Select(x =>
                new RateSubject()
                {
                    Id = x.Id,
                    Caption = x.Caption
                });
        }

        private STR_RateSubject GetMapped(RateSubject x)
        {
            return new STR_RateSubject()
            {
                Id = x.Id,
                Caption = x.Caption
            };
        }

        public List<RateSubject> Get(Expression<Func<RateSubject, bool>> filter = null)
        {
            try
            {
                var query = GetQuery();

                if (filter == null)
                    return query.ToList();

                return query.Where(filter).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public RateSubject SingleOrNull(Expression<Func<RateSubject, bool>> filter = null)
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

        public void Add(RateSubject item)
        {
            try
            {
                STR_RateSubject entity = GetMapped(item);

                db.STR_RateSubject.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(RateSubject item)
        {
            try
            {
                STR_RateSubject entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(RateSubject item)
        {
            try
            {
                STR_RateSubject entity = db.STR_RateSubject.Find(item.Id);

                if (entity != null)
                    db.STR_RateSubject.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<RateSubject, bool>> filter = null)
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
