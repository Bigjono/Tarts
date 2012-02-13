using System;
using System.Collections.Concurrent;

namespace Bronson.DB.Common.Resouce
{
    public static class QueryCacheController
    {
        #region private fields
        private static ConcurrentDictionary<string, string> _cacheItemDictionary = new ConcurrentDictionary<string, string>();
        #endregion

        #region public methods
        
        public static string Fetch(string fileName,  Func<string,string> getQueryMethod)
        {
            string key = fileName;
            EnsureCacheCreated();

            if (_cacheItemDictionary.ContainsKey(key))
            {
                var retVal = "";
                _cacheItemDictionary.TryGetValue(key, out retVal);
                return retVal;
            }

            return _cacheItemDictionary.GetOrAdd(key, getQueryMethod.Invoke(fileName));
        }

        #endregion


        #region helper
 

        private static void EnsureCacheCreated()
        {
            if(_cacheItemDictionary==null)
            {
                _cacheItemDictionary = new ConcurrentDictionary<string, string>();
            }

        }

        #endregion


    }
}
