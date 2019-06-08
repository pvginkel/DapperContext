using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;

namespace DapperContext.Test
{
    [TestFixture]
    public class SQLiteFixture
    {
        [Test]
        public void SimpleQuery()
        {
            using (var connection = OpenDatabase())
            using (var context = connection.OpenContext())
            {
                var customers = context.Query<Customer>("select RowId, * from Customer order by RowId").ToList();

                Assert.AreEqual(3, customers.Count);
                Assert.AreEqual("Customer 1", customers[0].Name);

                context.Commit();
            }
        }

        [Test]
        public void Rollback()
        {
            using (var connection = OpenDatabase())
            {
                using (var context = connection.OpenContext())
                {
                    context.Execute("delete from Customer");
                }

                using (var context = connection.OpenContext())
                {
                    int count = context.ExecuteScalar<int>("select count(*) from Customer");
                    Assert.AreEqual(3, count);
                }
            }
        }

        private SQLiteConnection OpenDatabase()
        {
            var connection = new SQLiteConnection("data source=:memory:");
            connection.Open();

            connection.Execute(@"
create table Customer
(
    Name text not null,
    Address text null
)
");

            var customers = new List<Customer>
            {
                new Customer
                {
                    Name = "Customer 1",
                    Address = "Address 1"
                },
                new Customer
                {
                    Name = "Customer 2",
                    Address = "Address 2"
                },
                new Customer
                {
                    Name = "Customer 3",
                    Address = "Address 3"
                },
            };

            foreach (var customer in customers)
            {
                connection.Execute("insert into Customer(Name, Address) values (@Name, @Address)", customer);
            }

            return connection;
        }

        private class Customer
        {
            public int RowId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }
}
