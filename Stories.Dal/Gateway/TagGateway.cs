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
    public class TagGateway : ITagGateway
    {
        private StoriesEntities db;

        public TagGateway()
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

        private IQueryable<Tag> GetQuery()
        {
            return db.STR_Tag.Select(x =>
                new Tag()
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    CreateDate = x.CreateDate
                });
        }

        private STR_Tag GetMapped(Tag x)
        {
            return new STR_Tag()
            {
                Id = x.Id,
                Caption = x.Caption,
                CreateDate = x.CreateDate
            };
        }

        public List<Tag> Get(Expression<Func<Tag, bool>> filter = null)
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

        public Tag SingleOrNull(Expression<Func<Tag, bool>> filter = null)
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

        public void Add(Tag item)
        {
            try
            {
                STR_Tag entity = GetMapped(item);

                db.STR_Tag.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(Tag item)
        {
            try
            {
                STR_Tag entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(Tag item)
        {
            try
            {
                STR_Tag entity = db.STR_Tag.Find(item.Id);

                if (entity != null)
                    db.STR_Tag.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<Tag, bool>> filter = null)
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
