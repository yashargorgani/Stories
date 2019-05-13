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
    public class UserAccountGateway : IUserAccountGateway
    {
        private StoriesEntities db;

        public UserAccountGateway()
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

        private IQueryable<UserAccount> GetQuery()
        {
            return db.PRF_UserAccount.Select(x =>
                new UserAccount()
                {
                    Id = x.Id,
                    UserProfileId = x.UserProfileId,
                    EmailAddress = x.EmailAddress,
                    Token = x.Token
                });
        }

        private PRF_UserAccount GetMapped(UserAccount x)
        {
            return new PRF_UserAccount()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                EmailAddress = x.EmailAddress,
                Token = x.Token
            };
        }

        public List<UserAccount> Get(Expression<Func<UserAccount, bool>> filter = null)
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

        public UserAccount SingleOrNull(Expression<Func<UserAccount, bool>> filter = null)
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

        public void Add(UserAccount item)
        {
            try
            {
                PRF_UserAccount entity = GetMapped(item);

                db.PRF_UserAccount.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(UserAccount item)
        {
            try
            {
                PRF_UserAccount entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(UserAccount item)
        {
            try
            {
                PRF_UserAccount entity = db.PRF_UserAccount.Find(item.Id);

                if (entity != null)
                    db.PRF_UserAccount.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<UserAccount, bool>> filter = null)
        {
            try
            {
                var query = GetQuery();

                return query.Where(filter ?? (x => true)).Count();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
