using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public class DbRetryFixedStrategy : IDbRetryStrategy
    {
        public int Retries { get; }
        public TimeSpan Delay { get; }

        public DbRetryFixedStrategy(int retries, TimeSpan delay)
        {
            Retries = retries;
            Delay = delay;
        }

        public IEnumerable<TimeSpan> GetIntervals()
        {
            for (int i = 0; i < Retries - 1; i++)
            {
                yield return Delay;
            }
        }
    }
}
