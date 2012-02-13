using System;
using System.Runtime.Caching;

namespace Bronson.DB.Common
{
    public class DefaultCacheService : Interface.ICacheService
    {
        public T Get<T>(string cacheId, Func<T> getItemCallback, int secondsToCache)
        {

            var item = MemoryCache.Default.Get(cacheId);
            if (item != null) return (T)item;
            item = new object();
            
            lock (item)
            {
                 
                item = getItemCallback();
                if (item == null) return default(T);
                var cacheItem = new CacheItem(cacheId) { Value = item };
                MemoryCache.Default.Add(cacheItem, GetCacheItemPoilcy(secondsToCache));
            }

            return (T)item;
        }

        public void Delete(string cacheId)
        {
            MemoryCache.Default.Remove(cacheId);
        }

        private static CacheItemPolicy GetCacheItemPoilcy(int secondsToCache)
        {
            // set the default to 60 seconds.
            if (secondsToCache == 0) secondsToCache = 60;
            return new CacheItemPolicy
                             {
                                 AbsoluteExpiration = DateTime.Now.AddSeconds(secondsToCache),
                                 Priority = CacheItemPriority.Default,
                             };


        }
    }
}
