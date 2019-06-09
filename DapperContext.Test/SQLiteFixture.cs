using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DbContext;
using NUnit.Framework;

namespace DapperContext.Test
{
    [TestFixture]
    public class SQLiteFixture : SQLiteFixtureBase
    {
        [Test]
        public void SimpleQuery()
        {
            var customers = Db.WithContext(context =>
            {
                return context.Query<Customer>("select RowId, * from Customer order by RowId").ToList();
            });

            Assert.AreEqual(3, customers.Count);
            Assert.AreEqual("Customer 1", customers[0].Name);
        }

        [Test]
        public void Rollback()
        {
            Db.WithContext(context =>
            {
                context.Execute("delete from Customer");

                context.Rollback();
            });

            int count = Db.WithContext(context =>
            {
                return context.ExecuteScalar<int>("select count(*) from Customer");
            });

            Assert.AreEqual(3, count);
        }
    }
}
