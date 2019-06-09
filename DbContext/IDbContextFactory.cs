using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbContext
{
    public interface IDbContextFactory
    {
        DbContextConfiguration Configuration { get; }

        IDbContext OpenContext();
        IDbContext OpenContext(IsolationLevel isolationLevel);
        Task<IDbContext> OpenContextAsync(CancellationToken cancellationToken = default);
        Task<IDbContext> OpenContextAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    }
}
