using System;
using Bronson.DB.Common.Model;
using Bronson.DB.Common.Resouce;

namespace Bronson.DB.Common.QueryProcessor
{
    public class FileQueryReader : IQueryReader
    {

        private readonly ResouceFileReader _resouceFileReader;

        #region constructor

        public FileQueryReader(Type forType, string defaultExtension="sql")
        {
            _resouceFileReader = new ResouceFileReader(forType, defaultExtension);
        }
        
        #endregion

        #region Public API
      
        public Query GetQuery(string queryName)
        {
            Query retVal = null;
            var queryText = _resouceFileReader.GetStringFromResouceFile(queryName);

            retVal = new Query()
                         {
                             SelectQuery = queryText,
                             CountQuery=""
                         };

            return retVal;
        }

        #endregion
      
    }
}
