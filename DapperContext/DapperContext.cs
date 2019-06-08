using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    public class DapperContext : IDisposable
    {
        private readonly IDbConnection connection;
        private IDbTransaction transaction;
        private bool committed;
        private bool disposed;

        public DapperContext(IDbConnection connection)
        {
            this.connection = connection;
            this.transaction = connection.BeginTransaction();
        }

        public DapperContext(IDbConnection connection, IsolationLevel isolationLevel)
        {
            this.connection = connection;
            this.transaction = connection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            committed = true;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (transaction != null)
                {
                    if (committed)
                        transaction.Commit();
                    else
                        transaction.Rollback();

                    transaction.Dispose();
                    transaction = null;
                }

                disposed = true;
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TReturn>(string sql, Type[] types, Func<Object[], TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TReturn>(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public int Execute(CommandDefinition command)
        {
            return connection.Execute(CreateCommandDefinition(command));
        }

        public object ExecuteScalar(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteScalar(sql, param, transaction, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public object ExecuteScalar(CommandDefinition command)
        {
            return connection.ExecuteScalar(CreateCommandDefinition(command));
        }

        public T ExecuteScalar<T>(CommandDefinition command)
        {
            return connection.ExecuteScalar<T>(CreateCommandDefinition(command));
        }

        public IDataReader ExecuteReader(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        }

        public IDataReader ExecuteReader(CommandDefinition command)
        {
            return connection.ExecuteReader(CreateCommandDefinition(command));
        }

        public IDataReader ExecuteReader(CommandDefinition command, CommandBehavior commandBehavior)
        {
            return connection.ExecuteReader(CreateCommandDefinition(command), commandBehavior);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public dynamic QueryFirst(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst(sql, param, transaction, commandTimeout, commandType);
        }

        public dynamic QueryFirstOrDefault(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault(sql, param, transaction, commandTimeout, commandType);
        }

        public dynamic QuerySingle(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle(sql, param, transaction, commandTimeout, commandType);
        }

        public dynamic QuerySingleOrDefault(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public T QueryFirst<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public T QuerySingle<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public T QuerySingleOrDefault<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<object> Query(Type type, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query(type, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public object QueryFirst(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst(type, sql, param, transaction, commandTimeout, commandType);
        }

        public object QueryFirstOrDefault(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault(type, sql, param, transaction, commandTimeout, commandType);
        }

        public object QuerySingle(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle(type, sql, param, transaction, commandTimeout, commandType);
        }

        public object QuerySingleOrDefault(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault(type, sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(CommandDefinition command)
        {
            return connection.Query<T>(CreateCommandDefinition(command));
        }

        public T QueryFirst<T>(CommandDefinition command)
        {
            return connection.QueryFirst<T>(CreateCommandDefinition(command));
        }

        public T QueryFirstOrDefault<T>(CommandDefinition command)
        {
            return connection.QueryFirstOrDefault<T>(CreateCommandDefinition(command));
        }

        public T QuerySingle<T>(CommandDefinition command)
        {
            return connection.QuerySingle<T>(CreateCommandDefinition(command));
        }

        public T QuerySingleOrDefault<T>(CommandDefinition command)
        {
            return connection.QuerySingleOrDefault<T>(CreateCommandDefinition(command));
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
        }

        public SqlMapper.GridReader QueryMultiple(CommandDefinition command)
        {
            return connection.QueryMultiple(CreateCommandDefinition(command));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TFirst, TSecond, TThird, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(CommandDefinition command)
        {
            return connection.QueryAsync(CreateCommandDefinition(command));
        }

        public Task<dynamic> QueryFirstAsync(CommandDefinition command)
        {
            return connection.QueryFirstAsync(CreateCommandDefinition(command));
        }

        public Task<dynamic> QueryFirstOrDefaultAsync(CommandDefinition command)
        {
            return connection.QueryFirstOrDefaultAsync(CreateCommandDefinition(command));
        }

        public Task<dynamic> QuerySingleAsync(CommandDefinition command)
        {
            return connection.QuerySingleAsync(CreateCommandDefinition(command));
        }

        public Task<dynamic> QuerySingleOrDefaultAsync(CommandDefinition command)
        {
            return connection.QuerySingleOrDefaultAsync(CreateCommandDefinition(command));
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<dynamic> QueryFirstAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<dynamic> QueryFirstOrDefaultAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefaultAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<dynamic> QuerySingleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<dynamic> QuerySingleOrDefaultAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefaultAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<object>> QueryAsync(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QueryFirstAsync(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QueryFirstOrDefaultAsync(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QuerySingleAsync(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition command)
        {
            return connection.QueryAsync<T>(CreateCommandDefinition(command));
        }

        public Task<IEnumerable<object>> QueryAsync(Type type, CommandDefinition command)
        {
            return connection.QueryAsync(type, CreateCommandDefinition(command));
        }

        public Task<object> QueryFirstAsync(Type type, CommandDefinition command)
        {
            return connection.QueryFirstAsync(type, CreateCommandDefinition(command));
        }

        public Task<T> QueryFirstAsync<T>(CommandDefinition command)
        {
            return connection.QueryFirstAsync<T>(CreateCommandDefinition(command));
        }

        public Task<object> QueryFirstOrDefaultAsync(Type type, CommandDefinition command)
        {
            return connection.QueryFirstOrDefaultAsync(type, CreateCommandDefinition(command));
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(CommandDefinition command)
        {
            return connection.QueryFirstOrDefaultAsync<T>(CreateCommandDefinition(command));
        }

        public Task<object> QuerySingleAsync(Type type, CommandDefinition command)
        {
            return connection.QuerySingleAsync(type, CreateCommandDefinition(command));
        }

        public Task<T> QuerySingleAsync<T>(CommandDefinition command)
        {
            return connection.QuerySingleAsync<T>(CreateCommandDefinition(command));
        }

        public Task<object> QuerySingleOrDefaultAsync(Type type, CommandDefinition command)
        {
            return connection.QuerySingleOrDefaultAsync(type, CreateCommandDefinition(command));
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(CommandDefinition command)
        {
            return connection.QuerySingleOrDefaultAsync<T>(CreateCommandDefinition(command));
        }

        public Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(CommandDefinition command)
        {
            return connection.ExecuteAsync(CreateCommandDefinition(command));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TReturn> map, string splitOn = "Id")
        {
            return connection.QueryAsync<TFirst, TSecond, TReturn>(CreateCommandDefinition(command), map, splitOn);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn = "Id")
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(CreateCommandDefinition(command), map, splitOn);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn = "Id")
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(CreateCommandDefinition(command), map, splitOn);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn = "Id")
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(CreateCommandDefinition(command), map, splitOn);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, string splitOn = "Id")
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(CreateCommandDefinition(command), map, splitOn);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, string splitOn = "Id")
        {
            return connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(CreateCommandDefinition(command), map, splitOn);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, Type[] types, Func<Object[], TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<TReturn>(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(CommandDefinition command)
        {
            return connection.QueryMultipleAsync(CreateCommandDefinition(command));
        }

        public Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IDataReader> ExecuteReaderAsync(CommandDefinition command)
        {
            return connection.ExecuteReaderAsync(CreateCommandDefinition(command));
        }

        public Task<IDataReader> ExecuteReaderAsync(CommandDefinition command, CommandBehavior commandBehavior)
        {
            return connection.ExecuteReaderAsync(CreateCommandDefinition(command), commandBehavior);
        }

        public Task<object> ExecuteScalarAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteScalarAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> ExecuteScalarAsync(CommandDefinition command)
        {
            return connection.ExecuteScalarAsync(CreateCommandDefinition(command));
        }

        public Task<T> ExecuteScalarAsync<T>(CommandDefinition command)
        {
            return connection.ExecuteScalarAsync<T>(CreateCommandDefinition(command));
        }


        private CommandDefinition CreateCommandDefinition(CommandDefinition command)
        {
            return new CommandDefinition(
                command.CommandText,
                command.Parameters,
                transaction,
                command.CommandTimeout,
                command.CommandType,
                command.Flags,
                command.CancellationToken
            );
        }
    }

    public static class DapperContextExtensions
    {
        public static DapperContext OpenContext(this IDbConnection connection)
        {
            return new DapperContext(connection);
        }

        public static DapperContext OpenContext(this IDbConnection connection, IsolationLevel isolationLevel)
        {
            return new DapperContext(connection, isolationLevel);
        }
    }
}
