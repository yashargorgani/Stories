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
    public class TagOfStoryGateway : ITagOfStoryGateway
    {
        private StoriesEntities db;

        public TagOfStoryGateway()
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

        private IQueryable<TagOfStory> GetQuery()
        {
            return db.STR_TagOfStory.Select(x =>
                new TagOfStory()
                {
                    Id = x.Id,
                    StoryId = x.StoryId,
                    TagId = x.TagId,

                    TagCaption = x.STR_Tag.Caption
                });
        }

        private STR_TagOfStory GetMapped(TagOfStory x)
        {
            return new STR_TagOfStory()
            {
                Id = x.Id,
                StoryId = x.StoryId,
                TagId = x.TagId,
            };
        }

        public List<TagOfStory> Get(Expression<Func<TagOfStory, bool>> filter = null)
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

        public TagOfStory SingleOrNull(Expression<Func<TagOfStory, bool>> filter = null)
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

        public void Add(TagOfStory item)
        {
            try
            {
                STR_TagOfStory entity = GetMapped(item);

                db.STR_TagOfStory.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(TagOfStory item)
        {
            try
            {
                STR_TagOfStory entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(TagOfStory item)
        {
            try
            {
                STR_TagOfStory entity = db.STR_TagOfStory.Find(item.Id);

                if (entity != null)
                    db.STR_TagOfStory.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<TagOfStory, bool>> filter = null)
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
