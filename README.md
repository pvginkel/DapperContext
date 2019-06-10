# DapperContext

[Dapper](<https://www.nuget.org/packages/Dapper/>) is a library from the StackOverflow people that simplifies working with database connections in .NET. Dapper is a very nice library, but it's missing one feature: automatic database transaction management.

Basically working with a transaction looks like this:

```cs
using (var transaction = connection.BeginTransaction())
{
    try
    {
        connection.Execute("delete from Customer", transaction: transaction);

        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}
```

The problem here is that it's very easy to forget the transaction parameter.

This project provides two NuGet packages to fix the above issue. The [SimpleDbContext](<https://www.nuget.org/packages/SimpleDbContext/>) NuGet package provides a simple abstraction to manage database connections. The [SimpleDapperContext](<https://www.nuget.org/packages/SimpleDapperContext/>) package provides extension methods to work with the core `IDbContext` abstraction provided by the SimpleDbContext package.

## Usage

Setup for SimpleDbContext is done using the `DbContextFactory` class. This class manages configuration and opening new database connections, e.g.:

```cs
var contextFactory = new DbContextFactory(
    () => new SQLiteConnection("data source=data.db")
);
```

The `DbContextFactory` takes an optional `DbContextConfiguration` instance. With this, you can configure the following:

* The retry policy allows you to implement automatic retry. There is an implementation for this named `DbAzureRetryPolicy` which retries on Azure transient failures. A retry policy itself takes a retry strategy. For this there is a fixed interval and an exponential back-off implementation. The following initializes a context factory with an Azure retry policy to do exponential back-off:

```cs
var contextFactory = new DbContextFactory(
    () => new SQLiteConnection("data source=data.db"),
    new DbContextConfiguration(
        retryPolicy: new DbAzureRetryPolicy(new DbRetryExponentialBackoffStrategy(5, TimeSpan.FromSeconds(0.5)))
    )
);
```

* The default isolation level specifies the `IsolationLevel` is none was specifically provided;
* The events allow you to provide an instance of `IDbContextEvents`. This allows you to hook in to specific events in a context lifecycle. This can be used in combination with the `IDbContext.Context` dictionary to manage states and actions with a context, e.g. to allow you to run some code after a transaction as committed successfully.

### Opening a database context

The `IDbContextFactory` interface has a method named `OpenContext`. This opens a new `IDbContext`. However, this is not the preferred usage and does not implement automatic retry. Instead, the `WithContext` and `WithContextAsync` methods should be used. E.g.:

```cs
contextFactory.WithContext(context =>
{
    context.Execute("delete from Customer");
});
```

The `WithContext` method also has an overload that returns a value, allowing you to e.g. do this:

```cs
var customers = contextFactory.WithContext(context =>
{
    return context.Query<Customer>("select * from Customer").ToList();
});
```

The `WithContext` method does a few things:

* If you do not explicitly call `IDbContext.Rollback`, the transaction will be committed automatically;
* If an exception occurs within the callback, and the `IDbRetryPolicy` is set, this policy will be used to automatically retry the whole callback. Do ensure that the callback allows for this can doesn't perform additional actions like calling a REST API or writing files to disk.

Note that the `WithContextAsync` methods implement `async` versions of `WithContext`. This takes a `CancellationToken` allowing you to cancel the retry. The `Commit` and `Rollback` operations themselves are the only methods that are not `async` simply because the .NET framework does not have `async` implementations for them. This is the same as what EntityFramework does, which will also run `Commit` and `Rollback` inline.

### Using Dapper

The SimpleDbContext NuGet only provides the `IDbContext` abstraction and functionality to work with thise. The SimpleDapperContext NuGet provides extension methods for `IDbContext`. Specifically it provides an implementation for all Dapper extension methods on `IDbConnection`. The difference between the standard Dapper extension methods and the ones provided by SimpleDapperContext is that the latter do not take a transaction parameter. Instead, this is automatically taken from `IDbContext.Transaction`.