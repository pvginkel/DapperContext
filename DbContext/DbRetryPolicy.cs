using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public class DbRetryPolicy : IDbRetryPolicy
    {
        public IDbRetryStrategy Strategy { get; }

        public DbRetryPolicy(IDbRetryStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            Strategy = strategy;
        }

        public virtual bool ShouldRetry(Exception exception)
        {
            return true;
        }
    }
}
