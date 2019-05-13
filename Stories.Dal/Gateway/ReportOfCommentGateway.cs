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
    public class ReportOfCommentGateway : IReportOfCommentGateway
    {
        private StoriesEntities db;

        public ReportOfCommentGateway()
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

        private IQueryable<ReportOfComment> GetQuery()
        {
            return db.STR_ReportOfComment.Select(x =>
                new ReportOfComment()
                {
                    Id = x.Id,
                    UserProfileId = x.UserProfileId,
                    CommentId = x.CommentId,
                    ReportSubjectId = x.ReportSubjectId,
                    ActionDate = x.ActionDate,

                    ReportSubjectCaption = x.STR_ReportSubject.Caption
                });
        }

        private STR_ReportOfComment GetMapped(ReportOfComment x)
        {
            return new STR_ReportOfComment()
            {
                Id = x.Id,
                UserProfileId = x.UserProfileId,
                CommentId = x.CommentId,
                ReportSubjectId = x.ReportSubjectId,
                ActionDate = x.ActionDate
            };
        }

        public List<ReportOfComment> Get(Expression<Func<ReportOfComment, bool>> filter = null)
        {
            try
            {
                var query = GetQuery();

                if (filter == null)
                    return query.ToList();

                return query.Where(filter).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ReportOfComment SingleOrNull(Expression<Func<ReportOfComment, bool>> filter = null)
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

        public void Add(ReportOfComment item)
        {
            try
            {
                STR_ReportOfComment entity = GetMapped(item);

                db.STR_ReportOfComment.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(ReportOfComment item)
        {
            try
            {
                STR_ReportOfComment entity = GetMapped(item);

                db.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(ReportOfComment item)
        {
            try
            {
                STR_ReportOfComment entity = db.STR_ReportOfComment.Find(item.Id);

                if (entity != null)
                    db.STR_ReportOfComment.Remove(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Count(Expression<Func<ReportOfComment, bool>> filter = null)
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
