using System;
using System.Collections;
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
            private readonly IDbContextEvents _events;
            private IDictionary _context;

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

            public IDictionary Context
            {
                get
                {
                    if (_context == null)
                        _context = new Hashtable();
                    return _context;
                }
            }

            public DbContext(IDbConnection connection, IDbTransaction transaction, IDbContextEvents events)
            {
                _connection = connection;
                _transaction = transaction;
                _events = events;

                _events?.Opened(this);
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
                        // If neither commit or rollback was called explicitly, we force
                        // a rollback.
                        if (CommitState == DbContextCommitState.None)
                            CommitState = DbContextCommitState.Rollback;

                        if (_events != null)
                        {
                            if (CommitState == DbContextCommitState.Commit)
                                _events.BeforeCommit(this);
                            else
                                _events.BeforeRollback(this);
                        }

                        using (_transaction)
                        {
                            if (CommitState == DbContextCommitState.Commit)
                                _transaction.Commit();
                            else
                                _transaction.Rollback();
                        }

                        _transaction = null;

                        if (_events != null)
                        {
                            if (CommitState == DbContextCommitState.Commit)
                                _events.AfterCommit(this);
                            else
                                _events.AfterRollback(this);
                        }
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }

                    _events?.Closed(this);

                    _disposed = true;
                }
            }
        }
    }
}
