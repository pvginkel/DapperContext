using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public class DbRetryExponentialBackoffStrategy : IDbRetryStrategy
    {
        public int Retries { get; }
        public TimeSpan MinDelay { get; }
        public TimeSpan? MaxDelay { get; }

        public DbRetryExponentialBackoffStrategy(int retries, TimeSpan minDelay, TimeSpan? maxDelay = null)
        {
            Retries = retries;
            MinDelay = minDelay;
            MaxDelay = maxDelay;
        }

        public IEnumerable<TimeSpan> GetIntervals()
        {
            var delay = MinDelay;

            for (int i = 0; i < Retries - 1; i++)
            {
                yield return delay;

                delay = new TimeSpan(delay.Ticks * 2);
                if (MaxDelay.HasValue)
                    delay = new TimeSpan(Math.Min(delay.Ticks, MaxDelay.Value.Ticks));
            }
        }
    }
}
