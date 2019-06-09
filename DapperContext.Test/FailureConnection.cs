using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperContext.Test
{
    public class FailureConnection : IDbConnection
    {
        private readonly FailureCallbacks _callbacks;

        public IDbConnection Owner { get; }

        public string ConnectionString
        {
            get => Owner.ConnectionString;
            set => Owner.ConnectionString = value;
        }

        public int ConnectionTimeout => Owner.ConnectionTimeout;

        public string Database => Owner.Database;

        public ConnectionState State => Owner.State;

        public FailureConnection(IDbConnection owner, FailureCallbacks callbacks)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (callbacks == null)
                throw new ArgumentNullException(nameof(callbacks));

            Owner = owner;
            _callbacks = callbacks;
        }

        public IDbTransaction BeginTransaction()
        {
            _callbacks.BeginTransaction?.Invoke();
            return new FailureTransaction(Owner.BeginTransaction(), _callbacks);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            _callbacks.BeginTransaction?.Invoke();
            return new FailureTransaction(Owner.BeginTransaction(il), _callbacks);
        }

        public void Close()
        {
            Owner.Close();
        }

        public void ChangeDatabase(string databaseName)
        {
            Owner.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            return new FailureCommand(Owner.CreateCommand(), _callbacks);
        }

        public void Open()
        {
            _callbacks.Open?.Invoke();
            Owner.Open();
        }

        public void Dispose()
        {
            Owner.Dispose();
        }
    }
}
