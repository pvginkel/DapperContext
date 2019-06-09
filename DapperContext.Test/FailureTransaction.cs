using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperContext.Test
{
    public class FailureTransaction : IDbTransaction
    {
        private readonly FailureCallbacks _callbacks;

        public IDbTransaction Owner { get; }

        public FailureTransaction(IDbTransaction owner, FailureCallbacks callbacks)
        {
            Owner = owner;
            _callbacks = callbacks;
        }

        public IDbConnection Connection => Owner.Connection;

        public IsolationLevel IsolationLevel => Owner.IsolationLevel;

        public void Commit()
        {
            _callbacks.CommitTransaction?.Invoke();
            Owner.Commit();
        }

        public void Rollback()
        {
            _callbacks.RollbackTransaction?.Invoke();
            Owner.Rollback();
        }

        public void Dispose()
        {
            Owner.Dispose();
        }
    }
}
