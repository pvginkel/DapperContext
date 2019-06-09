using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbContext
{
    public partial class DbContextFactory : IDbContextFactory
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public DbContextConfiguration Configuration { get; }

        public DbContextFactory(Func<IDbConnection> connectionFactory, DbContextConfiguration configuration = null)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException(nameof(connectionFactory));

            Configuration = configuration;
            _connectionFactory = connectionFactory;
        }

        public IDbContext OpenContext()
        {
            return OpenContext(null);
        }

        public IDbContext OpenContext(IsolationLevel isolationLevel)
        {
            return OpenContext((IsolationLevel?)isolationLevel);
        }

        private IDbContext OpenContext(IsolationLevel? isolationLevel)
        {
            if (!isolationLevel.HasValue)
                isolationLevel = Configuration?.DefaultIsolationLevel;

            var connection = _connectionFactory();

            connection.Open();

            var transaction = isolationLevel.HasValue
                ? connection.BeginTransaction(isolationLevel.Value)
                : connection.BeginTransaction();

            return new DbContext(connection, transaction);
        }

        public Task<IDbContext> OpenContextAsync(CancellationToken cancellationToken = default)
        {
            return OpenContextAsync(null, cancellationToken);
        }

        public Task<IDbContext> OpenContextAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            return OpenContextAsync((IsolationLevel?)isolationLevel, cancellationToken);
        }

        private async Task<IDbContext> OpenContextAsync(IsolationLevel? isolationLevel, CancellationToken cancellationToken = default)
        {
            if (!isolationLevel.HasValue)
                isolationLevel = Configuration?.DefaultIsolationLevel;

            var connection = _connectionFactory();

            await ((DbConnection)connection).OpenAsync(cancellationToken);

            var transaction = isolationLevel.HasValue
                ? connection.BeginTransaction(isolationLevel.Value)
                : connection.BeginTransaction();

            return new DbContext(connection, transaction);
        }
    }
}
