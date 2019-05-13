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
    public class EducationFieldGateway : IEducationFieldGateway
    {
        private StoriesEntities db;

        public EducationFieldGateway()
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

        private IQueryable<EducationField> GetQuery()
        {
            return db.PRF_EducationField.Select(x =>
                new EducationField()
                {
                    Id = x.Id,
                    Caption = x.Caption
                });
        }

        private PRF_EducationField GetMapped(EducationField x)
        {
            return new PRF_EducationField()
            {
                Id = x.Id,
                Caption = x.Caption
            };
        }

        public List<EducationField> Get(Expression<Func<EducationField, bool>> filter = null)
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

        public EducationField SingleOrNull(Expression<Func<EducationField, bool>> filter = null)
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

        public void Add(EducationField item)
        {
            try
            {
                PRF_EducationField entity = GetMapped(item);

                db.PRF_EducationField.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(EducationField item)
        {
            try
            {
                PRF_EducationField entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(EducationField item)
        {
            try
            {
                PRF_EducationField entity = db.PRF_EducationField.Find(item.Id);

                if (entity != null)
                    db.PRF_EducationField.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<EducationField, bool>> filter = null)
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
