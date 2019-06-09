using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public static class DbContextFactoryExtensions
    {
        public static void WithContext(this IDbContextFactory contextFactory, Action<IDbContext> action)
        {
            if (contextFactory == null)
                throw new ArgumentNullException(nameof(contextFactory));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var retryPolicy = contextFactory.Configuration?.RetryPolicy;
            if (retryPolicy != null)
                DbRetryer.Retry(retryPolicy, PerformAction);
            else
                PerformAction();

            void PerformAction()
            {
                using (var context = contextFactory.OpenContext())
                {
                    action(context);

                    if (context.CommitState != DbContextCommitState.Rollback)
                        context.Commit();
                }
            }
        }

        public static T WithContext<T>(this IDbContextFactory contextFactory, Func<IDbContext, T> action)
        {
            if (contextFactory == null)
                throw new ArgumentNullException(nameof(contextFactory));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var retryPolicy = contextFactory.Configuration?.RetryPolicy;
            if (retryPolicy != null)
                return DbRetryer.Retry(retryPolicy, PerformAction);
            else
                return PerformAction();

            T PerformAction()
            {
                T result;

                using (var context = contextFactory.OpenContext())
                {
                    result = action(context);

                    if (context.CommitState != DbContextCommitState.Rollback)
                        context.Commit();
                }

                return result;
            }
        }
    }
}
