using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbContext
{
    public static class DbRetryer
    {
        public static void Retry(IDbRetryPolicy policy, Action action)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            for (int i = 0; i < policy.Retries - 1; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex) when (policy.ShouldRetry(ex))
                {
                    Thread.Sleep(policy.Delay);
                }
            }

            action();
        }

        public static T Retry<T>(IDbRetryPolicy policy, Func<T> action)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            for (int i = 0; i < policy.Retries - 1; i++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex) when (policy.ShouldRetry(ex))
                {
                    Thread.Sleep(policy.Delay);
                }
            }

            return action();
        }
    }
}
