using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public interface IDbContextEvents
    {
        void Opened(IDbContext context);
        void BeforeCommit(IDbContext context);
        void AfterCommit(IDbContext context);
        void BeforeRollback(IDbContext context);
        void AfterRollback(IDbContext context);
        void Closed(IDbContext context);
    }
}
