using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContext;
using NUnit.Framework;

namespace DapperContext.Test
{
    public class SQLiteFixtureBase
    {
        private const string DbName = "test.db";

        protected readonly IDbContextFactory Db;

        public SQLiteFixtureBase()
        {
            Db = CreateContextFactory();
        }

        protected virtual IDbContextFactory CreateContextFactory()
        {
            return new DbContextFactory(CreateConnection);
        }

        protected IDbConnection CreateConnection()
        {
            return new SQLiteConnection("data source=" + DbName);
        }

        [SetUp]
        public void SetUp()
        {
            if (File.Exists(DbName))
                File.Delete(DbName);

            Db.WithContext(context =>
            {
                context.Execute(@"
create table Customer
(
    Name text not null,
    Address text null
)
");

                var customers = new[]
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
                    context.Execute("insert into Customer(Name, Address) values (@Name, @Address)", customer);
                }
            });
        }
    }
}
