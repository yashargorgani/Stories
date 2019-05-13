using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Stories.Dal.DataEntity;
using System.Threading.Tasks;

namespace Stories.Dal
{
    public class UnitOfWork : IDisposable
    {
        private List<IGateway> _gateways;

        public StoriesEntities dbContext { get; set; }

        public UnitOfWork(params IGateway[] gateways)
        {
            _gateways = new List<IGateway>();
            if (gateways != null)
            {
                gateways.ToList().ForEach(p =>
                {
                    this.AddGateway(p);
                });
            }
        }
        
        public void AddGateway(IGateway gateway)
        {
            if (!_gateways.Contains(gateway))
            {
                _gateways.Add(gateway);
                gateway.Unit = this;
            }
        }

        public void SubmitChanges()
        {
            dbContext.SaveChanges();
        }

        public async Task SubmitChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
