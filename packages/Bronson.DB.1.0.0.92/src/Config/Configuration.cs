using System;
using System.Collections.Concurrent;
using System.Configuration;
using Bronson.DB.Common;
using Bronson.DB.Common.Interface;

namespace Bronson.DB.Config
{
    public class Configuration
    {
        #region constructors
        public Configuration()
        {
            ConnectionString = "";
            ProviderName = "";
            ParsingRules = new ConcurrentDictionary<string, string>();
            CacheService =new DefaultCacheService();
            CurrentContextMode = CurrentContextModes.Web;
        }
        #endregion




        #region fluent methods

        public Configuration WithConnectionSettingsFromConfig(string connectionStringName)
        {
            if (ConfigurationManager.ConnectionStrings[connectionStringName] == null)
                throw new InvalidOperationException("Can't find a connection string with the name '" + connectionStringName + "'");

            
            if (HasProviderNameInSystemConfigFile(connectionStringName))
                    this.ProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;

            this.ConnectionString =ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return this;
        }

        public Configuration AddQueryParserRule(string lookfor,string replaceWith)
        {
            var replaceValue = "";

            if (!ParsingRules.TryGetValue(lookfor, out replaceValue))
                        ParsingRules[lookfor]= replaceWith;    
            
            return this;

        }
  
        public Configuration OverrideCacheServiceWith(ICacheService cacheService)
        {
            this.CacheService = cacheService;
            return this;
        }

        public Configuration WithContextModeOf(CurrentContextModes requestedContextType)
        {
            this.CurrentContextMode = requestedContextType;
            return this;
        }

        #endregion

        public enum CurrentContextModes
        {
            Web,
            Static
        }

        #region properties


        internal CurrentContextModes CurrentContextMode;
        internal string ConnectionString;
        internal string ProviderName;
        internal ConcurrentDictionary<string, string> ParsingRules;
        internal ICacheService CacheService;
        #endregion


        #region helpers
        private static bool HasProviderNameInSystemConfigFile(string connectionStringName)
        {
            return !string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName);
        }

        #endregion

    }
}
