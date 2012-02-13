using System.Data;
 
using Bronson.DB.Common.Interface;
using Dapper;

namespace Bronson.DB.Common
{
    public class Writer
    {
        #region dependancy
        private readonly IDBManager _dbManager;
        #endregion

        #region constructor
        public Writer(IDBManager dbManager)
        {
            _dbManager = dbManager;
        }
        #endregion

        #region write method wrapper

        public int ExecuteCommand(string sql, object param = null, IDbTransaction transaction = null)
        {
            _dbManager.CheckAndOpenConnection();

            var retVal = _dbManager.Connection.Execute(_dbManager.Parser.ParseString(sql), param, transaction);

            _dbManager.CheckAndCloseConnection();

            return retVal;
        }




        #endregion


    }
}
