using System;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using Bronson.DB.Common.ContextHandler;
using Bronson.DB.Common.Interface;
using Bronson.DB.Config;
using Bronson.DB.Util;

namespace Bronson.DB.Common
{
    
    public class DBManager : IDBManager
    {
        #region depenedancy
        public Parser Parser { get; private set; }
        #endregion

        #region Properties

        public IDbConnection Connection 
        { 
            get
            {
                var _connection = GetConnectionFromContext();
                if (_connection != null) return _connection;
                
                _connection = CreateConnection();
               
                
                
                return _connection;

            } 
        }

        public ICacheService CacheService { get { return _configuration.CacheService; } }
         
        #endregion

        #region private vars
        private Configuration _configuration;
        private DbProviderFactory _factory;
        private bool _implicitConnection;
        internal string ContextKey;
        #endregion

         
        #region Configuration

        public void ConfigureDBManager(Configuration configuration)
        {
            _configuration = configuration;
            Parser = new Parser(configuration.ParsingRules);

            ContextKey = GenerateHashForConnection(configuration.ConnectionString);
        }

        #endregion

        #region connection management

        public IDbConnection CreateConnection()
        {
            if (_factory == null) SetupDaoFactory();

            var newConnection = _factory.CreateConnection();
            newConnection.ConnectionString = _configuration.ConnectionString;
            SetCurrentConnection(newConnection);
            return newConnection;
        }

        public void SetCurrentConnection(IDbConnection connection )
        {
            
            if (_configuration.CurrentContextMode == Configuration.CurrentContextModes.Web){
                WebContextFactory.Set(ContextKey,connection);
                    return;
            }

            StaticContextFactory.Set(ContextKey,connection);
        }

        public void OpenConnection()
        {
            if (Connection != null && Connection.State == ConnectionState.Open) return;
            if (Connection != null) Connection.Open();
        }

        public void CloseConnection()
        {
            if (Connection.State != ConnectionState.Open) return;

            Connection.Close();
        }

        public void CheckAndOpenConnection()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                _implicitConnection = false;
                return;
            }

            OpenConnection();

            _implicitConnection = true;
        }

        public void CheckAndCloseConnection()
        {
            if (!_implicitConnection) return;
            if (Connection == null) return;

            CloseConnection();

            _implicitConnection = false;
        }

        #endregion




        #region Internal Methods

        

        #endregion

        #region private helper

        private void SetupDaoFactory()
        {
            _factory = DbProviderFactories.GetFactory(_configuration.ProviderName);
        }

        private IDbConnection GetConnectionFromContext()
        {

            if (_configuration.CurrentContextMode == Configuration.CurrentContextModes.Web)
                return WebContextFactory.CurrentConnection(ContextKey);

            return StaticContextFactory.CurrentConnection(ContextKey);
        }

        public static string GenerateHashForConnection(string connectionString)
        {
            var retVal = new StringBuilder(connectionString);
            var hasher = new MD5CryptoServiceProvider();
            byte[] data = Encoding.ASCII.GetBytes(retVal.ToString());
            var hash = Convert.ToBase64String(hasher.ComputeHash(data));
            return hash;
        }

        #endregion
    }
}
