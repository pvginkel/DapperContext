using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public class DbRetryPolicy : IDbRetryPolicy
    {
        public int Retries { get; }
        public TimeSpan Delay { get; }

        public DbRetryPolicy(int retries, TimeSpan delay)
        {
            Retries = retries;
            Delay = delay;
        }

        public virtual bool ShouldRetry(Exception exception)
        {
            return true;
        }
    }
}
