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
    public class UserProfileGateway : IUserProfileGateway
    {
        private StoriesEntities db;

        public UserProfileGateway()
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

        private IQueryable<UserProfile> GetQuery()
        {
            return db.PRF_UserProfile.Select(x =>
                new UserProfile()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Bio = x.Bio,
                    MobileNumber = x.MobileNumber,
                    ProfileImage = x.ProfileImage,
                    RegisterDate = x.RegisterDate,
                });
        }

        private PRF_UserProfile GetMapped(UserProfile x)
        {
            return new PRF_UserProfile()
            {
                Id = x.Id,
                Name = x.Name,
                Bio = x.Bio,
                MobileNumber = x.MobileNumber,
                ProfileImage = x.ProfileImage,
                RegisterDate = x.RegisterDate
            };
        }

        public List<UserProfile> Get(Expression<Func<UserProfile, bool>> filter = null)
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

        public UserProfile SingleOrNull(Expression<Func<UserProfile, bool>> filter = null)
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

        public void Add(UserProfile item)
        {
            try
            {
                PRF_UserProfile entity = GetMapped(item);

                db.PRF_UserProfile.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(UserProfile item)
        {
            try
            {
                PRF_UserProfile entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(UserProfile item)
        {
            try
            {
                PRF_UserProfile entity = db.PRF_UserProfile.Find(item.Id);

                if (entity != null)
                    db.PRF_UserProfile.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<UserProfile, bool>> filter = null)
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
