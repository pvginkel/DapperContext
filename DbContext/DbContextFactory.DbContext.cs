using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    partial class DbContextFactory
    {
        private class DbContext : IDbContext
        {
            private bool _disposed;
            private IDbConnection _connection;
            private IDbTransaction _transaction;

            public IDbConnection Connection
            {
                get
                {
                    VerifyNotDisposed();
                    return _connection;
                }
            }

            public IDbTransaction Transaction
            {
                get
                {
                    VerifyNotDisposed();
                    return _transaction;
                }
            }

            public DbContextCommitState CommitState { get; private set; }

            public DbContext(IDbConnection connection, IDbTransaction transaction)
            {
                _connection = connection;
                _transaction = transaction;
            }

            public void Commit()
            {
                VerifyNotDisposed();

                CommitState = DbContextCommitState.Commit;
            }

            public void Rollback()
            {
                VerifyNotDisposed();

                CommitState = DbContextCommitState.Rollback;
            }

            private void VerifyNotDisposed()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().Name);
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    if (_transaction != null)
                    {
                        using (_transaction)
                        {
                            if (CommitState == DbContextCommitState.Commit)
                                _transaction.Commit();
                            else
                                _transaction.Rollback();
                        }

                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }

                    _disposed = true;
                }
            }
        }
    }
}
