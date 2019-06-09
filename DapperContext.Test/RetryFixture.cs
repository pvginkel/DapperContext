using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContext;
using NUnit.Framework;

namespace DapperContext.Test
{
    [TestFixture]
    public class RetryFixture : SQLiteFixtureBase
    {
        private readonly FailureCallbacks _callbacks = new FailureCallbacks();

        protected override IDbContextFactory CreateContextFactory()
        {
            return new DbContextFactory(
                () => new FailureConnection(CreateConnection(), _callbacks),
                new DbContextConfiguration(
                    new DbAzureRetryPolicy(3, default)
                )
            );
        }

        [Test]
        public void SingleRetry()
        {
            int iteration = 0;

            Db.WithContext(context =>
            {
                if (++iteration > 1)
                    _callbacks.ExecuteCommand = null;
                else
                    _callbacks.ExecuteCommand = () => throw new TimeoutException();

                context.Execute("delete from Customer");
            });

            Assert.AreEqual(2, iteration);

            int count = Db.WithContext(context =>
            {
                return context.ExecuteScalar<int>("select count(*) from Customer");
            });

            Assert.AreEqual(0, count);
        }

        [Test]
        public void Failure()
        {
            _callbacks.ExecuteCommand = () => throw new TimeoutException();

            bool failed = false;
            int iteration = 0;

            try
            {
                Db.WithContext(context =>
                {
                    iteration++;
                    context.Execute("delete from Customer");
                });
            }
            catch
            {
                failed = true;
            }

            _callbacks.ExecuteCommand = null;

            Assert.IsTrue(failed);

            Assert.AreEqual(3, iteration);
        }
    }
}
