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
    public class UserTopicGateway : IUserTopicGateway
    {
        private StoriesEntities db;

        public UserTopicGateway()
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

        private IQueryable<UserTopic> GetQuery()
        {
            return db.STR_UserTopic.Select(x =>
            new UserTopic()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                TopicId = x.TopicId,

                TopicCaption = x.STR_Topic.Caption
            });
        }

        private STR_UserTopic GetMapped(UserTopic x)
        {
            return new STR_UserTopic()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                TopicId = x.TopicId,
            };
        }

        public List<UserTopic> Get(Expression<Func<UserTopic, bool>> filter = null)
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

        public UserTopic SingleOrNull(Expression<Func<UserTopic, bool>> filter = null)
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

        public void Add(UserTopic item)
        {
            try
            {
                STR_UserTopic entity = GetMapped(item);

                db.STR_UserTopic.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(UserTopic item)
        {
            try
            {
                STR_UserTopic entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(UserTopic item)
        {
            try
            {
                STR_UserTopic entity = db.STR_UserTopic.Find(item.Id);

                if (entity != null)
                    db.STR_UserTopic.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<UserTopic, bool>> filter = null)
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
