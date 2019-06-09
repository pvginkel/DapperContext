using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperContext.Test
{
    public class FailureCommand : IDbCommand
    {
        private readonly FailureCallbacks _callbacks;

        public IDbCommand Owner { get; }

        public FailureCommand(IDbCommand owner, FailureCallbacks callbacks)
        {
            Owner = owner;
            _callbacks = callbacks;
        }

        public IDbConnection Connection
        {
            get => Owner.Connection;
            set => Owner.Connection = ((FailureConnection)value).Owner;
        }

        public IDbTransaction Transaction
        {
            get => Owner.Transaction;
            set => Owner.Transaction = ((FailureTransaction)value).Owner;
        }

        public string CommandText
        {
            get => Owner.CommandText;
            set => Owner.CommandText = value;
        }

        public int CommandTimeout
        {
            get => Owner.CommandTimeout;
            set => Owner.CommandTimeout = value;
        }

        public CommandType CommandType
        {
            get => Owner.CommandType;
            set => Owner.CommandType = value;
        }

        public IDataParameterCollection Parameters => Owner.Parameters;

        public UpdateRowSource UpdatedRowSource
        {
            get => Owner.UpdatedRowSource;
            set => Owner.UpdatedRowSource = value;
        }

        public void Prepare()
        {
            Owner.Prepare();
        }

        public void Cancel()
        {
            Owner.Cancel();
        }

        public IDbDataParameter CreateParameter()
        {
            return Owner.CreateParameter();
        }

        public int ExecuteNonQuery()
        {
            _callbacks.ExecuteCommand?.Invoke();
            return Owner.ExecuteNonQuery();
        }

        public IDataReader ExecuteReader()
        {
            _callbacks.ExecuteCommand?.Invoke();
            return Owner.ExecuteReader();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            _callbacks.ExecuteCommand?.Invoke();
            return Owner.ExecuteReader(behavior);
        }

        public object ExecuteScalar()
        {
            _callbacks.ExecuteCommand?.Invoke();
            return Owner.ExecuteScalar();
        }

        public void Dispose()
        {
            Owner.Dispose();
        }
    }
}
