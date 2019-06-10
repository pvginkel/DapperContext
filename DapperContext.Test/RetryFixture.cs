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
                    new DbAzureRetryPolicy(new DbRetryFixedStrategy(3, default))
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

            int iteration = 0;

            try
            {
                Db.WithContext(context =>
                {
                    iteration++;
                    context.Execute("delete from Customer");
                });

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<TimeoutException>(ex);
            }
            finally
            {
                _callbacks.ExecuteCommand = null;
            }

            Assert.AreEqual(3, iteration);
        }

        [Test]
        public async Task SingleRetryAsync()
        {
            int iteration = 0;

            await Db.WithContextAsync(async context =>
            {
                if (++iteration > 1)
                    _callbacks.ExecuteCommand = null;
                else
                    _callbacks.ExecuteCommand = () => throw new TimeoutException();

                await context.ExecuteAsync("delete from Customer");
            });

            Assert.AreEqual(2, iteration);

            int count = await Db.WithContextAsync(async context =>
            {
                return await context.ExecuteScalarAsync<int>("select count(*) from Customer");
            });

            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task FailureAsync()
        {
            _callbacks.ExecuteCommand = () => throw new TimeoutException();

            int iteration = 0;

            try
            {
                await Db.WithContextAsync(async context =>
                {
                    iteration++;
                    await context.ExecuteAsync("delete from Customer");
                });

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<TimeoutException>(ex);
            }
            finally
            {
                _callbacks.ExecuteCommand = null;
            }

            Assert.AreEqual(3, iteration);
        }
    }
}
