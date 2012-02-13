using System;
using System.Data;

namespace Dapper
{
    public static partial class SqlMapper
    {
        #region Execute Extensions
        public static object ExecuteScaler(
#if CSHARP30
            this IDbConnection cnn, string sql, object param, IDbTransaction transaction, int? commandTimeout, CommandType? commandType
#else
this IDbConnection cnn, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
#endif

)
        {
            CacheInfo info = null;
            // nice and simple
            Identity identity = new Identity(sql, commandType, cnn, null, (object)param == null ? null : ((object)param).GetType(), null);
            info = GetCacheInfo(identity);
            return ExecuteScaler(cnn, transaction, sql, info.ParamReader, (object)param, commandTimeout, commandType);
        }

        private static object ExecuteScaler(IDbConnection cnn, IDbTransaction tranaction, string sql, Action<IDbCommand, object> paramReader, object obj, int? commandTimeout, CommandType? commandType)
        {
            using (var cmd = SetupCommand(cnn, tranaction, sql, paramReader, obj, commandTimeout, commandType))
            {
                return cmd.ExecuteScalar();
            }

        }


        #endregion



    }

}