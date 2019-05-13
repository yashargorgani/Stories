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
    public class CommentOfStoryGateway : ICommentOfStoryGateway
    {
        private StoriesEntities db;

        public CommentOfStoryGateway()
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

        private IQueryable<CommentOfStory> GetQuery()
        {
            return db.STR_CommentOfStory.Select(x => 
            new CommentOfStory()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                StoryId = x.StoryId,
                Comment = x.Comment,
                IsReply = x.IsReply,
                ReplyToId = x.ReplyToId,
                ActionDate = x.ActionDate,
            });
        }

        private STR_CommentOfStory GetMapped(CommentOfStory x)
        {
            return new STR_CommentOfStory()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                StoryId = x.StoryId,
                Comment = x.Comment,
                IsReply = x.IsReply,
                ReplyToId = x.ReplyToId,
                ActionDate = x.ActionDate,
            };
        }

        public int Count(Expression<Func<CommentOfStory, bool>> filter = null)
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

        public List<CommentOfStory> Get(Expression<Func<CommentOfStory, bool>> filter = null)
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

        public CommentOfStory SingleOrNull(Expression<Func<CommentOfStory, bool>> filter = null)
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

        public void Add(CommentOfStory item)
        {
            try
            {
                STR_CommentOfStory entity = GetMapped(item);

                db.STR_CommentOfStory.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(CommentOfStory item)
        {
            try
            {
                STR_CommentOfStory entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(CommentOfStory item)
        {
            try
            {
                STR_CommentOfStory entity = db.STR_CommentOfStory.Find(item.Id);

                if (entity != null)
                    db.STR_CommentOfStory.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
