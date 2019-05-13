using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class RateOfStoryGateway : IRateOfStoryGateway
    {
        private StoriesEntities db;

        public RateOfStoryGateway()
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

        private IQueryable<RateOfStory> GetQuery()
        {
            return db.STR_RateOfStory.Select(x =>
                new RateOfStory()
                {
                    Id = x.Id,
                    UserProfileId = x.UserProfileId,
                    RateSubjectId = x.RateSubjectId,
                    Rate = x.Rate,
                    ActionDate = x.ActionDate,

                    RateSubjectCaption = x.STR_RateSubject.Caption
                });
        }

        private STR_RateOfStory GetMapped(RateOfStory x)
        {
            return new STR_RateOfStory()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                RateSubjectId = x.RateSubjectId,
                Rate = x.Rate,
                ActionDate = x.ActionDate
            };
        }

        public List<RateOfStory> Get(Expression<Func<RateOfStory, bool>> filter = null)
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

        public RateOfStory SingleOrNull(Expression<Func<RateOfStory, bool>> filter = null)
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

        public void Add(RateOfStory item)
        {
            try
            {
                STR_RateOfStory entity = GetMapped(item);

                db.STR_RateOfStory.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(RateOfStory item)
        {
            try
            {
                STR_RateOfStory entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(RateOfStory item)
        {
            try
            {
                STR_RateOfStory entity = db.STR_RateOfStory.Find(item.Id);

                if (entity != null)
                    db.STR_RateOfStory.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<RateOfStory, bool>> filter = null)
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
