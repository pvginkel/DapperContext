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

            IEnumerator<TimeSpan> intervals = null;

            while (true)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex) when (policy.ShouldRetry(ex))
                {
                    if (intervals == null)
                        intervals = policy.Strategy.GetIntervals().GetEnumerator();
                    if (!intervals.MoveNext())
                        throw;
                    Thread.Sleep(intervals.Current);
                }
            }
        }

        public static T Retry<T>(IDbRetryPolicy policy, Func<T> action)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            IEnumerator<TimeSpan> intervals = null;

            while (true)
            {
                try
                {
                    return action();
                }
                catch (Exception ex) when (policy.ShouldRetry(ex))
                {
                    if (intervals == null)
                        intervals = policy.Strategy.GetIntervals().GetEnumerator();
                    if (!intervals.MoveNext())
                        throw;
                    Thread.Sleep(intervals.Current);
                }
            }
        }

        public static async Task RetryAsync(IDbRetryPolicy policy, Func<Task> action, CancellationToken cancellationToken = default)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            IEnumerator<TimeSpan> intervals = null;

            while (true)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex) when (policy.ShouldRetry(ex))
                {
                    if (intervals == null)
                        intervals = policy.Strategy.GetIntervals().GetEnumerator();
                    if (!intervals.MoveNext())
                        throw;
                    await Task.Delay(intervals.Current, cancellationToken);
                }
            }
        }

        public static async Task<T> RetryAsync<T>(IDbRetryPolicy policy, Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            IEnumerator<TimeSpan> intervals = null;

            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex) when (policy.ShouldRetry(ex))
                {
                    if (intervals == null)
                        intervals = policy.Strategy.GetIntervals().GetEnumerator();
                    if (!intervals.MoveNext())
                        throw;
                    await Task.Delay(intervals.Current, cancellationToken);
                }
            }
        }
    }
}
