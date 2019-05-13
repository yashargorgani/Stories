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
    public class ReportSubjectGateway : IReportSubjectGateway
    {
        private StoriesEntities db;

        public ReportSubjectGateway()
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

        private IQueryable<ReportSubject> GetQuery()
        {
            return db.STR_ReportSubject.Select(x =>
                new ReportSubject()
                {
                    Id = x.Id,
                    Caption = x.Caption
                });
        }

        private STR_ReportSubject GetMapped(ReportSubject x)
        {
            return new STR_ReportSubject()
            {
                Id = x.Id,
                Caption = x.Caption
            };
        }

        public List<ReportSubject> Get(Expression<Func<ReportSubject, bool>> filter = null)
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

        public ReportSubject SingleOrNull(Expression<Func<ReportSubject, bool>> filter = null)
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

        public void Add(ReportSubject item)
        {
            try
            {
                STR_ReportSubject entity = GetMapped(item);

                db.STR_ReportSubject.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(ReportSubject item)
        {
            try
            {
                STR_ReportSubject entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(ReportSubject item)
        {
            try
            {
                STR_ReportSubject entity = db.STR_ReportSubject.Find(item.Id);

                if (entity != null)
                    db.STR_ReportSubject.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<ReportSubject, bool>> filter = null)
        {
            try
            {
                if (filter == null)
                    return db.STR_ReportSubject.Count();

                var query = from entity in db.STR_ReportSubject
                    select new ReportSubject();

                return query.Count(filter);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
