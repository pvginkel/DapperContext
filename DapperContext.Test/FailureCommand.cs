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
    public class FailureCommand : DbCommand
    {
        private readonly FailureCallbacks _callbacks;

        public IDbCommand Owner { get; }

        public FailureCommand(IDbCommand owner, FailureCallbacks callbacks)
        {
            Owner = owner;
            _callbacks = callbacks;
        }

        protected override DbConnection DbConnection
        {
            get => (DbConnection)Owner.Connection;
            set => Owner.Connection = ((FailureConnection)value).Owner;
        }

        protected override DbParameterCollection DbParameterCollection => (DbParameterCollection)Owner.Parameters;

        protected override DbTransaction DbTransaction
        {
            get => (DbTransaction)Owner.Transaction;
            set => Owner.Transaction = ((FailureTransaction)value).Owner;
        }

        public override bool DesignTimeVisible { get; set; }

        public override string CommandText
        {
            get => Owner.CommandText;
            set => Owner.CommandText = value;
        }

        public override int CommandTimeout
        {
            get => Owner.CommandTimeout;
            set => Owner.CommandTimeout = value;
        }

        public override CommandType CommandType
        {
            get => Owner.CommandType;
            set => Owner.CommandType = value;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get => Owner.UpdatedRowSource;
            set => Owner.UpdatedRowSource = value;
        }

        public override void Prepare()
        {
            Owner.Prepare();
        }

        public override void Cancel()
        {
            Owner.Cancel();
        }

        protected override DbParameter CreateDbParameter()
        {
            return (DbParameter)Owner.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            _callbacks.ExecuteCommand?.Invoke();
            return (DbDataReader)Owner.ExecuteReader(behavior);
        }

        public override int ExecuteNonQuery()
        {
            _callbacks.ExecuteCommand?.Invoke();
            return Owner.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            _callbacks.ExecuteCommand?.Invoke();
            return Owner.ExecuteScalar();
        }

        public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            _callbacks.ExecuteCommand?.Invoke();
            return ((DbCommand)Owner).ExecuteNonQueryAsync(cancellationToken);
        }

        protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            _callbacks.ExecuteCommand?.Invoke();
            return ((DbCommand)Owner).ExecuteReaderAsync(behavior, cancellationToken);
        }

        public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            _callbacks.ExecuteCommand?.Invoke();
            return ((DbCommand)Owner).ExecuteScalarAsync(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Owner.Dispose();
        }
    }
}
