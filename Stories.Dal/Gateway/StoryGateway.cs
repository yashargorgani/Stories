using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stories.Models;
using Stories.Dal.DataEntity;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Stories.Dal.Interface;
using System.Diagnostics;
using Stories.Helpers;

namespace Stories.Dal.Gateway
{
    public class StoryGateway : IStoryGateway
    {
        private StoriesEntities db;

        public StoryGateway()
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

        private IQueryable<Story> GetQuery()
        {
            return db.STR_Story.Select(x =>
                new Story()
                {
                    Id = x.Id,
                    UserProfileId = x.UserProfileId,
                    Title = x.Title,
                    Body = x.Body,
                    TopicId = x.TopicId,
                    StoryImage = x.StoryImage,
                    ActionDate = x.ActionDate,
                    StoryStatusId = x.StoryStatusId,

                    StoryStatusCaption = x.STR_StoryStatus.Caption,
                    StoryStatusValue = x.STR_StoryStatus.Value,
                    TopicCaption = x.STR_Topic.Caption,
                    UserProfileName = x.PRF_UserProfile.Name,
                    UserProfileImage = x.PRF_UserProfile.ProfileImage
                });
        }

        private STR_Story GetMapped(Story x)
        {
            return new STR_Story()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                Title = x.Title,
                Body = x.Body,
                TopicId = x.TopicId,
                StoryImage = x.StoryImage,
                StoryStatusId = x.StoryStatusId,
                ActionDate = x.ActionDate,
            };
        }

        public List<Story> Get(Expression<Func<Story, bool>> filter = null)
        {
            try
            {
                var query = GetQuery();

                var list = query.Where(filter ?? (x => true)).ToList();

                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Story SingleOrNull(Expression<Func<Story, bool>> filter = null)
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

        public void Add(Story item)
        {
            try
            {
                STR_Story entity = GetMapped(item);

                db.STR_Story.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(Story item)
        {
            try
            {
                STR_Story entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(Story item)
        {
            try
            {
                STR_Story entity = db.STR_Story.Find(item.Id);

                if (entity != null)
                    db.STR_Story.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<Story, bool>> filter = null)
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

        public List<Story> GetPaged(int page, Expression<Func<Story, bool>> filter = null)
        {
            try
            {
                var query = GetQuery();
                var count = 10;

                var list =
                    query.Where(filter ?? (x => true))
                    .OrderByDescending(x => x.ActionDate)
                    .Skip((page - 1) * count).Take(count).ToList();

                Parallel.ForEach(list.Where(x => x.StoryImage != null),
                    item =>
                {
                    item.StoryImage = item.StoryImage.ReduceSize();
                });

                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
