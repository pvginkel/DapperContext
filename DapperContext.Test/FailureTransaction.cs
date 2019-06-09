using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperContext.Test
{
    public class FailureTransaction : DbTransaction
    {
        private readonly FailureCallbacks _callbacks;

        public IDbTransaction Owner { get; }

        public FailureTransaction(IDbTransaction owner, FailureCallbacks callbacks)
        {
            Owner = owner;
            _callbacks = callbacks;
        }

        protected override DbConnection DbConnection => (DbConnection)Owner.Connection;

        public override IsolationLevel IsolationLevel => Owner.IsolationLevel;

        public override void Commit()
        {
            _callbacks.CommitTransaction?.Invoke();
            Owner.Commit();
        }

        public override void Rollback()
        {
            _callbacks.RollbackTransaction?.Invoke();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Owner.Dispose();
        }
    }
}
