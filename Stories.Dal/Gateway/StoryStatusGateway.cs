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
    public class StoryStatusGateway : IStoryStatusGateway
    {
        private StoriesEntities db;

        public StoryStatusGateway()
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

        private IQueryable<StoryStatus> GetQuery()
        {
            return db.STR_StoryStatus.Select(x =>
                new StoryStatus()
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    Value = x.Value
                });
        }

        private STR_StoryStatus GetMapped(StoryStatus x)
        {
            return new STR_StoryStatus()
            {
                Id = x.Id,
                Caption = x.Caption,
                Value = x.Value
            };
        }

        public List<StoryStatus> Get(Expression<Func<StoryStatus, bool>> filter = null)
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

        public StoryStatus SingleOrNull(Expression<Func<StoryStatus, bool>> filter = null)
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

        public void Add(StoryStatus item)
        {
            throw new NotImplementedException();
        }

        public void Update(StoryStatus item)
        {
            throw new NotImplementedException();
        }

        public void Remove(StoryStatus item)
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<StoryStatus, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
