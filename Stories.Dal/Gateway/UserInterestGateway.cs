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
    public class UserInterestGateway : IUserInterestGateway
    {
        private StoriesEntities db;

        public UserInterestGateway()
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

        private IQueryable<UserInterest> GetQuery()
        {
            return db.PRF_UserInterest.Select(x =>
                new UserInterest()
                {
                    Id = x.Id,
                    InterestId = x.InterestId,
                    UserProfileId = x.UserProfileId,

                    InterestCaption = x.PRF_Interest.Caption
                });
        }

        private PRF_UserInterest GetMapped(UserInterest x)
        {
            return new PRF_UserInterest()
            {
                Id = x.Id,
                InterestId = x.InterestId,
                UserProfileId = x.UserProfileId
            };
        }

        public List<UserInterest> Get(Expression<Func<UserInterest, bool>> filter = null)
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

        public UserInterest SingleOrNull(Expression<Func<UserInterest, bool>> filter = null)
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

        public void Add(UserInterest item)
        {
            try
            {
                PRF_UserInterest entity = GetMapped(item);  

                db.PRF_UserInterest.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(UserInterest item)
        {
            try
            {
                PRF_UserInterest entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(UserInterest item)
        {
            try
            {
                PRF_UserInterest entity = db.PRF_UserInterest.Find(item.Id);

                if (entity != null)
                    db.PRF_UserInterest.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<UserInterest, bool>> filter = null)
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
