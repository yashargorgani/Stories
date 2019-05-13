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
    public class ReportOfStoryGateway : IReportOfStoryGateway
    {
        private StoriesEntities db;

        public ReportOfStoryGateway()
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

        private IQueryable<ReportOfStory> GetQuery()
        {
            return db.STR_ReportOfStory.Select(x =>
                new ReportOfStory()
                {
                    Id = x.Id,
                    UserProfileId = x.UserProfileId,
                    StoryId = x.StoryId,
                    ReportSubjectId = x.ReportSubjectId,
                    ActionDate = x.ActionDate,

                    ReportSubjectCaption = x.STR_ReportSubject.Caption,
                    StoryTitle = x.STR_Story.Title
                });
        }

        private STR_ReportOfStory GetMapped(ReportOfStory x)
        {
            return new STR_ReportOfStory()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                StoryId = x.StoryId,
                ReportSubjectId = x.ReportSubjectId,
                ActionDate = x.ActionDate
            };
        }

        public List<ReportOfStory> Get(Expression<Func<ReportOfStory, bool>> filter = null)
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

        public ReportOfStory SingleOrNull(Expression<Func<ReportOfStory, bool>> filter = null)
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

        public void Add(ReportOfStory item)
        {
            try
            {
                STR_ReportOfStory entity = GetMapped(item);

                db.STR_ReportOfStory.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(ReportOfStory item)
        {
            try
            {
                STR_ReportOfStory entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(ReportOfStory item)
        {
            try
            {
                STR_ReportOfStory entity = db.STR_ReportOfStory.Find(item.Id);

                if (entity != null)
                    db.STR_ReportOfStory.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<ReportOfStory, bool>> filter = null)
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
