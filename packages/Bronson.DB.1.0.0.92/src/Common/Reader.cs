using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Bronson.DB.Common.Interface;
using Bronson.DB.Common.QueryProcessor.Mapper;
using Dapper;

namespace Bronson.DB.Common
{
    public class Reader
    {

        #region dependacy

        private readonly IDBManager _dbManager;

        #endregion

        #region constructor

        public Reader(IDBManager dbManager)
        {
            _dbManager = dbManager;
        }
       
        #endregion


        #region Reader Execute Methods  (Static Type)

        public T ExecuteSingle<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            _dbManager.CheckAndOpenConnection();

             var queryResults = _dbManager.Connection.Query<T>(_dbManager.Parser.ParseString(sql), param, transaction);
             var retVal = queryResults.Take(1).SingleOrDefault();
            
            _dbManager.CheckAndCloseConnection();

            return retVal;


        }

        public IEnumerable<T> ExecuteSQL<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            _dbManager.CheckAndOpenConnection();

            var retVal = _dbManager.Connection.Query<T>(_dbManager.Parser.ParseString(sql), param, transaction);

            _dbManager.CheckAndCloseConnection();

            return retVal;
        }

        public SqlMapper.GridReader ExecuteMultiple(string sql, object param = null, IDbTransaction transaction = null)
        {
            return this._dbManager.Connection.QueryMultiple(_dbManager.Parser.ParseString(sql), param, transaction);
        }

        public T ExecuteCachedSingle<T>(string sql, object param = null, int seconds = 60, IDbTransaction transaction = null)  
        {
            return _dbManager.CacheService.Get(GenerateStatementHash(sql, param), ()=> ExecuteSingle<T>(sql,param,transaction),seconds);
        }

        public IEnumerable<T> ExecuteCachedSQL<T>(string sql, object param = null, int seconds = 60, IDbTransaction transaction = null)
        {

            return _dbManager.CacheService.Get(GenerateStatementHash(sql, param),() => ExecuteSQL<T>(sql, param, transaction),seconds);
        }


        #endregion

        #region Reader Execute Methods (Dynamic)

        public dynamic ExecuteSingle(string sql, object param = null, IDbTransaction transaction = null)
        {
            _dbManager.CheckAndOpenConnection();

            var queryResults = _dbManager.Connection.Query(_dbManager.Parser.ParseString(sql), param, transaction);

            var retVal = queryResults.Take(1).SingleOrDefault();
           
            _dbManager.CheckAndCloseConnection();

            return retVal;
        }

        public IEnumerable<dynamic> ExecuteSQL(string sql, object param = null,IDbTransaction transaction = null)
        {
            _dbManager.CheckAndOpenConnection();

            var retVal = _dbManager.Connection.Query<object>(_dbManager.Parser.ParseString(sql), param, transaction);

            _dbManager.CheckAndCloseConnection();

            return retVal;
        }

        #endregion



        #region Scaler
        public object ExecuteScaler(string sql, object param=null)
        {
            _dbManager.CheckAndOpenConnection();

            var retVal =_dbManager.Connection.ExecuteScaler(_dbManager.Parser.ParseString(sql), param);

            _dbManager.CheckAndCloseConnection();

            return retVal;
        }

        #endregion

        #region cachedQueries

        public static string GenerateStatementHash(string statement, object param)
        {
            var retVal = new StringBuilder(statement);
            retVal.AppendFormat(ParametersHelper.GetParamInfoToString(param));
            
            var hasher = new MD5CryptoServiceProvider();
            byte[] data = Encoding.ASCII.GetBytes(retVal.ToString());
            var hash = Convert.ToBase64String(hasher.ComputeHash(data));
            return hash;
        }

        #endregion
    }
}
