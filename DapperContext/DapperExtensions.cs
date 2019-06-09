using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContext;
using Dapper;

namespace DapperContext
{
    public static class DapperExtensions
    {
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TReturn>(this IDbContext context, string sql, Type[] types, Func<Object[], TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<TReturn>(sql, types, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static int Execute(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Execute(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static int Execute(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.Execute(CreateCommandDefinition(context, command));
        }

        public static object ExecuteScalar(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.ExecuteScalar(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static T ExecuteScalar<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.ExecuteScalar<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static object ExecuteScalar(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.ExecuteScalar(CreateCommandDefinition(context, command));
        }

        public static T ExecuteScalar<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.ExecuteScalar<T>(CreateCommandDefinition(context, command));
        }

        public static IDataReader ExecuteReader(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.ExecuteReader(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static IDataReader ExecuteReader(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.ExecuteReader(CreateCommandDefinition(context, command));
        }

        public static IDataReader ExecuteReader(this IDbContext context, CommandDefinition command, CommandBehavior commandBehavior)
        {
            return context.Connection.ExecuteReader(CreateCommandDefinition(context, command), commandBehavior);
        }

        public static IEnumerable<dynamic> Query(this IDbContext context, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query(sql, param, context.Transaction, buffered, commandTimeout, commandType);
        }

        public static dynamic QueryFirst(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirst(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static dynamic QueryFirstOrDefault(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstOrDefault(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static dynamic QuerySingle(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingle(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static dynamic QuerySingleOrDefault(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleOrDefault(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static IEnumerable<T> Query<T>(this IDbContext context, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<T>(sql, param, context.Transaction, buffered, commandTimeout, commandType);
        }

        public static T QueryFirst<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirst<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstOrDefault<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static T QuerySingle<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingle<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleOrDefault<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static IEnumerable<object> Query(this IDbContext context, Type type, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query(type, sql, param, context.Transaction, buffered, commandTimeout, commandType);
        }

        public static object QueryFirst(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirst(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static object QueryFirstOrDefault(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstOrDefault(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static object QuerySingle(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingle(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static object QuerySingleOrDefault(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleOrDefault(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static IEnumerable<T> Query<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.Query<T>(CreateCommandDefinition(context, command));
        }

        public static T QueryFirst<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryFirst<T>(CreateCommandDefinition(context, command));
        }

        public static T QueryFirstOrDefault<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryFirstOrDefault<T>(CreateCommandDefinition(context, command));
        }

        public static T QuerySingle<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QuerySingle<T>(CreateCommandDefinition(context, command));
        }

        public static T QuerySingleOrDefault<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QuerySingleOrDefault<T>(CreateCommandDefinition(context, command));
        }

        public static SqlMapper.GridReader QueryMultiple(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryMultiple(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static SqlMapper.GridReader QueryMultiple(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryMultiple(CreateCommandDefinition(context, command));
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<TFirst, TSecond, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<TFirst, TSecond, TThird, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<dynamic>> QueryAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<dynamic>> QueryAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryAsync(CreateCommandDefinition(context, command));
        }

        public static Task<dynamic> QueryFirstAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryFirstAsync(CreateCommandDefinition(context, command));
        }

        public static Task<dynamic> QueryFirstOrDefaultAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryFirstOrDefaultAsync(CreateCommandDefinition(context, command));
        }

        public static Task<dynamic> QuerySingleAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QuerySingleAsync(CreateCommandDefinition(context, command));
        }

        public static Task<dynamic> QuerySingleOrDefaultAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QuerySingleOrDefaultAsync(CreateCommandDefinition(context, command));
        }

        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<T> QueryFirstAsync<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstAsync<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<T> QueryFirstOrDefaultAsync<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<T> QuerySingleAsync<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleAsync<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<T> QuerySingleOrDefaultAsync<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleOrDefaultAsync<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<dynamic> QueryFirstAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<dynamic> QueryFirstOrDefaultAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstOrDefaultAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<dynamic> QuerySingleAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<dynamic> QuerySingleOrDefaultAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleOrDefaultAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<object>> QueryAsync(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<object> QueryFirstAsync(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstAsync(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<object> QueryFirstOrDefaultAsync(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryFirstOrDefaultAsync(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<object> QuerySingleAsync(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleAsync(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<object> QuerySingleOrDefaultAsync(this IDbContext context, Type type, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QuerySingleOrDefaultAsync(type, sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryAsync<T>(CreateCommandDefinition(context, command));
        }

        public static Task<IEnumerable<object>> QueryAsync(this IDbContext context, Type type, CommandDefinition command)
        {
            return context.Connection.QueryAsync(type, CreateCommandDefinition(context, command));
        }

        public static Task<object> QueryFirstAsync(this IDbContext context, Type type, CommandDefinition command)
        {
            return context.Connection.QueryFirstAsync(type, CreateCommandDefinition(context, command));
        }

        public static Task<T> QueryFirstAsync<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryFirstAsync<T>(CreateCommandDefinition(context, command));
        }

        public static Task<object> QueryFirstOrDefaultAsync(this IDbContext context, Type type, CommandDefinition command)
        {
            return context.Connection.QueryFirstOrDefaultAsync(type, CreateCommandDefinition(context, command));
        }

        public static Task<T> QueryFirstOrDefaultAsync<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryFirstOrDefaultAsync<T>(CreateCommandDefinition(context, command));
        }

        public static Task<object> QuerySingleAsync(this IDbContext context, Type type, CommandDefinition command)
        {
            return context.Connection.QuerySingleAsync(type, CreateCommandDefinition(context, command));
        }

        public static Task<T> QuerySingleAsync<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QuerySingleAsync<T>(CreateCommandDefinition(context, command));
        }

        public static Task<object> QuerySingleOrDefaultAsync(this IDbContext context, Type type, CommandDefinition command)
        {
            return context.Connection.QuerySingleOrDefaultAsync(type, CreateCommandDefinition(context, command));
        }

        public static Task<T> QuerySingleOrDefaultAsync<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QuerySingleOrDefaultAsync<T>(CreateCommandDefinition(context, command));
        }

        public static Task<int> ExecuteAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.ExecuteAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<int> ExecuteAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.ExecuteAsync(CreateCommandDefinition(context, command));
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IDbContext context, CommandDefinition command, Func<TFirst, TSecond, TReturn> map, string splitOn = "Id")
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TReturn>(CreateCommandDefinition(context, command), map, splitOn);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(this IDbContext context, CommandDefinition command, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn = "Id")
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(CreateCommandDefinition(context, command), map, splitOn);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbContext context, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn = "Id")
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(CreateCommandDefinition(context, command), map, splitOn);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbContext context, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn = "Id")
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(CreateCommandDefinition(context, command), map, splitOn);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbContext context, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, string splitOn = "Id")
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(CreateCommandDefinition(context, command), map, splitOn);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbContext context, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbContext context, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, string splitOn = "Id")
        {
            return context.Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(CreateCommandDefinition(context, command), map, splitOn);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TReturn>(this IDbContext context, string sql, Type[] types, Func<Object[], TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryAsync<TReturn>(sql, types, map, param, context.Transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<SqlMapper.GridReader> QueryMultipleAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.QueryMultipleAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<SqlMapper.GridReader> QueryMultipleAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.QueryMultipleAsync(CreateCommandDefinition(context, command));
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.ExecuteReaderAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.ExecuteReaderAsync(CreateCommandDefinition(context, command));
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbContext context, CommandDefinition command, CommandBehavior commandBehavior)
        {
            return context.Connection.ExecuteReaderAsync(CreateCommandDefinition(context, command), commandBehavior);
        }

        public static Task<object> ExecuteScalarAsync(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.ExecuteScalarAsync(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<T> ExecuteScalarAsync<T>(this IDbContext context, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return context.Connection.ExecuteScalarAsync<T>(sql, param, context.Transaction, commandTimeout, commandType);
        }

        public static Task<object> ExecuteScalarAsync(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.ExecuteScalarAsync(CreateCommandDefinition(context, command));
        }

        public static Task<T> ExecuteScalarAsync<T>(this IDbContext context, CommandDefinition command)
        {
            return context.Connection.ExecuteScalarAsync<T>(CreateCommandDefinition(context, command));
        }


        private static CommandDefinition CreateCommandDefinition(IDbContext context, CommandDefinition command)
        {
            return new CommandDefinition(
                command.CommandText,
                command.Parameters,
                context.Transaction,
                command.CommandTimeout,
                command.CommandType,
                command.Flags,
                command.CancellationToken
            );
        }
    }
}
