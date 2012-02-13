using System;
using System.Collections.Generic;
using System.Data;
using Bronson.DB.Common;
using Bronson.DB.Common.Exceptions;
using Bronson.DB.Common.Interface;
using Bronson.DB.Common.Model;
using Bronson.DB.Common.QueryProcessor;
using Dapper;

namespace Bronson.DB
{
    public class QueryController : IDisposable
    {

        #region dependancy

        private readonly IQueryReader _xmlQueryReader;
        private readonly IQueryReader _fileQueryReader;
        private readonly Reader _dbReader;

        #endregion

        #region constructor

        public QueryController(IDBManager dbManager, Type callingType)
        {
            // default to XML Reader
            _xmlQueryReader = new XMLQueryReader(callingType);
            _fileQueryReader = new FileQueryReader(callingType);

            _dbReader = new Reader(dbManager);

        }

        #endregion



        #region Typed queries

        public IEnumerable<T> Query<T>(string queryName, object param = null, int secondsToCacheFor = 0)
        {
            if (secondsToCacheFor > 0)
                return _dbReader.ExecuteCachedSQL<T>(GetStoreQueryText(queryName), param, seconds: secondsToCacheFor);

            return _dbReader.ExecuteSQL<T>(GetStoreQueryText(queryName), param);
        }

        public T QuerySingle<T>(string queryName, object param = null, int secondsToCacheFor = 0)
        {

            if (secondsToCacheFor > 0)
                return _dbReader.ExecuteCachedSingle<T>(GetStoreQueryText(queryName), param, seconds: secondsToCacheFor);


            return _dbReader.ExecuteSingle<T>(GetStoreQueryText(queryName), param);
        }

        public SqlMapper.GridReader QueryMulitple(string queryName, object param = null)
        {
            return _dbReader.ExecuteMultiple(GetStoreQueryText(queryName), param);

        }

        #endregion

        #region dynamic queries

        public IEnumerable<dynamic> Query(string queryName, object param = null)
        {
            return _dbReader.ExecuteSQL(GetStoreQueryText(queryName), param);

        }

        public dynamic QuerySingle(string queryName, object param = null)
        {
            return _dbReader.ExecuteSingle(GetStoreQueryText(queryName), param);
        }

        #endregion



        #region Typed Specification Queries

        public List<T> SpecQuery<T>(string queryName, dynamic specification = null, int secondsToCacheFor = 0)
        {
            var query = GetParsed(queryName, specification);

            if (secondsToCacheFor > 0)
                return _dbReader.ExecuteCachedSQL<T>(query.SelectQuery, specification);

            return _dbReader.ExecuteSQL<T>(query.SelectQuery, specification);

        }

        public T SpecQuerySingle<T>(string queryName, dynamic specification = null, int secondsToCacheFor = 0)
        {
            var query = GetParsed(queryName, specification);

            if (secondsToCacheFor > 0)
                return _dbReader.ExecuteCachedSingle<T>(query.SelectQuery, specification, seconds: secondsToCacheFor);

            return _dbReader.ExecuteSingle<T>(query.SelectQuery, specification);

        }

        #endregion



        public object Scalar(string queryName, object param = null)
        {
            return _dbReader.ExecuteScaler(GetStoreQueryText(queryName), param);
        }


        public object SpecScalar(string queryName, dynamic specification = null)
        {
            var query = GetParsed(queryName, specification);

            return _dbReader.ExecuteScaler(query.SelectQuery, specification);
        }

        public object QueryScalarSQL(string sql, object param = null)
        {

            return _dbReader.ExecuteScaler(sql, param);
        }

        #region helper

        private Query GetParsed(string queryName, dynamic specification)
        {
            var query = GetQueryFromStore(queryName);
            query.ParseSelectQuery(specification);
            return query;
        }

        private string GetStoreQueryText(string queryName)
        {
            var query = GetQueryFromStore(queryName);
            return query.SelectQuery;
        }

        private Query GetQueryFromStore(string queryName)
        {

            try
            {
                return _xmlQueryReader.GetQuery(queryName);
            }
            catch (QueryResouceNotFoundException)
            {
                return _fileQueryReader.GetQuery(queryName);
            }


        }

        #endregion





        #region Typed queries

        public IEnumerable<T> QuerySQL<T>(string sql, object param = null, int secondsToCacheFor = 0)
        {

            if (secondsToCacheFor > 0)
                return _dbReader.ExecuteCachedSQL<T>(sql, param, seconds: secondsToCacheFor);

            return _dbReader.ExecuteSQL<T>(sql, param);
        }

        public T QuerySingleSQL<T>(string sql, object param = null, int secondsToCacheFor = 0)
        {
            if (secondsToCacheFor > 0)
                return _dbReader.ExecuteCachedSingle<T>(sql, param, seconds: secondsToCacheFor);

            return _dbReader.ExecuteSingle<T>(sql, param);
        }

        #endregion

        #region dynamic queries

        public IEnumerable<dynamic> QuerySQL(string sql, object param = null)
        {
            return _dbReader.ExecuteSQL(sql, param);
        }

        public dynamic QuerySingleSQL(string sql, object param = null)
        {
            return _dbReader.ExecuteSingle(sql, param);
        }

        #endregion



        public void Dispose()
        {

        }

    }

}
