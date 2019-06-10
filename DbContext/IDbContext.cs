using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        DbContextCommitState CommitState { get; }
        IDictionary Context { get; }

        void Commit();
        void Rollback();
    }
}
