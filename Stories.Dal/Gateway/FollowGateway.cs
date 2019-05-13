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
    public class FollowGateway : IFollowGateway
    {
        private StoriesEntities db;

        public FollowGateway()
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

        private IQueryable<Follow> GetQuery()
        {
            return db.PRF_Follow.Select(x =>
                new Follow()
                {
                    Id = x.Id,
                    FollowedUserId = x.FollowedUserId,
                    FollowingUserId = x.FollowingUserId,
                    ActionDate = x.ActionDate
                });
        }

        private PRF_Follow GetMapped(Follow x)
        {
            return new PRF_Follow()
            {
                Id = x.Id,
                FollowedUserId = x.FollowedUserId,
                FollowingUserId = x.FollowingUserId,
                ActionDate = x.ActionDate
            };
        }

        public List<Follow> Get(Expression<Func<Follow, bool>> filter = null)
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

        public Follow SingleOrNull(Expression<Func<Follow, bool>> filter = null)
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

        public void Add(Follow item)
        {
            try
            {
                PRF_Follow entity = GetMapped(item);

                db.PRF_Follow.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(Follow item)
        {
            try
            {
                PRF_Follow entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(Follow item)
        {
            try
            {
                PRF_Follow entity = db.PRF_Follow.Find(item.Id);

                if (entity != null)
                    db.PRF_Follow.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<Follow, bool>> filter = null)
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
