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
    public class LikeOfStoryGateway : ILikeOfStoryGateway
    {
        private StoriesEntities db;

        public LikeOfStoryGateway()
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

        private IQueryable<LikeOfStory> GetQuery()
        {
            return db.STR_LikeOfStory.Select(x => 
            new LikeOfStory()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                StoryId = x.StoryId,
                Value = x.Value,
                ActionDate = x.ActionDate,
            });
        }

        private STR_LikeOfStory GetMapped(LikeOfStory x)
        {
            return new STR_LikeOfStory()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                StoryId = x.StoryId,
                Value = x.Value,
                ActionDate = x.ActionDate,
            };
        }

        public int Count(Expression<Func<LikeOfStory, bool>> filter = null)
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

        public List<LikeOfStory> Get(Expression<Func<LikeOfStory, bool>> filter = null)
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

        public LikeOfStory SingleOrNull(Expression<Func<LikeOfStory, bool>> filter = null)
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

        public void Add(LikeOfStory item)
        {
            try
            {
                STR_LikeOfStory entity = GetMapped(item);

                db.STR_LikeOfStory.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(LikeOfStory item)
        {
            try
            {
                STR_LikeOfStory entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(LikeOfStory item)
        {
            try
            {
                STR_LikeOfStory entity = db.STR_LikeOfStory.Find(item.Id);

                if (entity != null)
                    db.STR_LikeOfStory.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
