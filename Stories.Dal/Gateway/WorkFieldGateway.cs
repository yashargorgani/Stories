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
    public class WorkFieldGateway : IWorkFieldGateway
    {
        private StoriesEntities db;

        public WorkFieldGateway()
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

        private IQueryable<WorkField> GetQuery()
        {
            return db.PRF_WorkField.Select(x => 
            new WorkField()
            {
                Id = x.Id,
                Caption = x.Caption
            });
        }

        private PRF_WorkField GetMapped(WorkField x)
        {
            return new PRF_WorkField()
            {
                Id = x.Id,
                Caption = x.Caption
            };
        }

        public List<WorkField> Get(Expression<Func<WorkField, bool>> filter = null)
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

        public WorkField SingleOrNull(Expression<Func<WorkField, bool>> filter = null)
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

        public void Add(WorkField item)
        {
            try
            {
                PRF_WorkField entity = GetMapped(item);

                db.PRF_WorkField.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(WorkField item)
        {
            try
            {
                PRF_WorkField entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(WorkField item)
        {
            try
            {
                PRF_WorkField entity = db.PRF_WorkField.Find(item.Id);

                if (entity != null)
                    db.PRF_WorkField.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<WorkField, bool>> filter = null)
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
