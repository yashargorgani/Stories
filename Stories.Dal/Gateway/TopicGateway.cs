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
using System.Diagnostics;

namespace Stories.Dal.Gateway
{
    public class TopicGateway : ITopicGateway
    {
        private StoriesEntities db;

        public TopicGateway()
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

        private IQueryable<Topic> GetQuery()
        {
            return db.STR_Topic.Select(x =>
                new Topic()
                {
                    Id = x.Id,
                    Caption = x.Caption
                });
        }

        private STR_Topic GetMapped(Topic x)
        {
            return new STR_Topic()
            {
                Id = x.Id,
                Caption = x.Caption
            };
        }

        public List<Topic> Get(Expression<Func<Topic, bool>> filter = null)
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

        public Topic SingleOrNull(Expression<Func<Topic, bool>> filter = null)
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

        public void Add(Topic item)
        {
            try
            {
                STR_Topic entity = GetMapped(item);

                db.STR_Topic.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(Topic item)
        {
            try
            {
                STR_Topic entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(Topic item)
        {
            try
            {
                STR_Topic entity = db.STR_Topic.Find(item.Id);

                if (entity != null)
                    db.STR_Topic.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<Topic, bool>> filter = null)
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

        public List<Topic> GetFavorites(int count)
        {
            try
            {
                var favs = 
                    db.STR_Topic
                        .Select(x => new { Id = x.Id, Count = x.STR_UserTopic.Count() })
                            .OrderBy(x => x.Count).Take(count).Select(x => x.Id);

                return db.STR_Topic.Where(x => favs.Contains(x.Id)).Select(x =>
                            new Topic()
                            {
                                Id = x.Id,
                                Caption = x.Caption
                            }).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
