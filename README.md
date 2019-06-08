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

This project provides a `DapperContext` class that abstracts this away. The above code would look as follows:

```cs
using (var context = connection.OpenContext())
{
    context.Execute("delete from Customer");
    
    context.Commit();
}
```

`DapperContext` does two things: pass a transaction everywhere and automate rollback. Passing the transaction everywhere is done by providing the same methods as Dapper provides, but without the transaction parameter. Then, the method calls the base Dapper method with the transaction added. If the method takes a `CommandDefinition`, this is rewritten to include the transaction.

The second is done by having the `DapperContext` implement `IDisposable` and calling `Commit` or `Rollback` when disposing the instance. The `Commit` method on `DapperContext` doesn't actually commit. Instead, it sets a flag. Then, in the `Dispose` method, this flag is checked ensuring that `Commit` or `Rollback` is called always.

## Usage

`DapperContext` is provided as a T4 template. You use this as follows:

* In your project, create a file named `DapperContext.tt`;
* Paste the contents of the `DapperContext.tt` file into this file.

The T4 template automatically finds the `Dapper.dll` file and generates `DapperContext` from it. The advantage of this is that when you update `Dapper`, you can rerun the T4 template and get access to new methods automatically.