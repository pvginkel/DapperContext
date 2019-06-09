using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DapperContext.Test
{
    public class FailureConnection : DbConnection
    {
        private readonly FailureCallbacks _callbacks;

        public IDbConnection Owner { get; }

        public override string ConnectionString
        {
            get => Owner.ConnectionString;
            set => Owner.ConnectionString = value;
        }

        public override int ConnectionTimeout => Owner.ConnectionTimeout;

        public override string Database => Owner.Database;

        public override string DataSource => ((DbConnection)Owner).DataSource;

        public override string ServerVersion => ((DbConnection)Owner).ServerVersion;

        public override ConnectionState State => Owner.State;

        public FailureConnection(IDbConnection owner, FailureCallbacks callbacks)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (callbacks == null)
                throw new ArgumentNullException(nameof(callbacks));

            Owner = owner;
            _callbacks = callbacks;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            _callbacks.BeginTransaction?.Invoke();
            return new FailureTransaction(Owner.BeginTransaction(isolationLevel), _callbacks);
        }

        public override void Close()
        {
            Owner.Close();
        }

        public override void ChangeDatabase(string databaseName)
        {
            Owner.ChangeDatabase(databaseName);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new FailureCommand(Owner.CreateCommand(), _callbacks);
        }

        public override void Open()
        {
            _callbacks.Open?.Invoke();
            Owner.Open();
        }

        public override Task OpenAsync(CancellationToken cancellationToken)
        {
            _callbacks.Open?.Invoke();
            return ((DbConnection)Owner).OpenAsync(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Owner.Dispose();
        }
    }
}
