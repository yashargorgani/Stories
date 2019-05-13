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
    public class EducationLevelGateway : IEducationLevelGateway
    {
        private StoriesEntities db;
        
        public EducationLevelGateway()
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

        private IQueryable<EducationLevel> GetQuery()
        {
            return db.PRF_EducationLevel.Select(x =>
                new EducationLevel()
                {
                    Id = x.Id,
                    Caption = x.Caption
                });
        }

        private PRF_EducationLevel GetMapped(EducationLevel x)
        {
            return new PRF_EducationLevel()
            {
                Id = x.Id,
                Caption = x.Caption
            };
        }

        public List<EducationLevel> Get(Expression<Func<EducationLevel, bool>> filter = null)
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

        public EducationLevel SingleOrNull(Expression<Func<EducationLevel, bool>> filter = null)
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

        public void Add(EducationLevel item)
        {
            try
            {
                PRF_EducationLevel entity = GetMapped(item);

                db.PRF_EducationLevel.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(EducationLevel item)
        {
            try
            {
                PRF_EducationLevel entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(EducationLevel item)
        {
            try
            {
                PRF_EducationLevel entity = db.PRF_EducationLevel.Find(item.Id);

                if (entity != null)
                    db.PRF_EducationLevel.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<EducationLevel, bool>> filter = null)
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
