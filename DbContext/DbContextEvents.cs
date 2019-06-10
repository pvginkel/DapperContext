using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public class DbContextEvents : IDbContextEvents
    {
        public virtual void Opened(IDbContext context)
        {
        }

        public virtual void BeforeCommit(IDbContext context)
        {
        }

        public virtual void AfterCommit(IDbContext context)
        {
        }

        public virtual void BeforeRollback(IDbContext context)
        {
        }

        public virtual void AfterRollback(IDbContext context)
        {
        }

        public virtual void Closed(IDbContext context)
        {
        }
    }
}
