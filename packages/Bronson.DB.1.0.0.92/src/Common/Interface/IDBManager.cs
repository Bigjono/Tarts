using System.Data;
using Bronson.DB.Config;
using Bronson.DB.Util;

namespace Bronson.DB.Common.Interface
{
    public interface IDBManager
    {

        Parser Parser { get; }
        IDbConnection Connection { get; }
        ICacheService CacheService { get; }

        void ConfigureDBManager(Configuration configuration);
        
        IDbConnection CreateConnection();
        void SetCurrentConnection(IDbConnection connection);
        void OpenConnection();
        void CloseConnection();
        void CheckAndOpenConnection();
        void CheckAndCloseConnection();
    }

}
