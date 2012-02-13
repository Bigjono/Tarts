using System;

namespace Bronson.DB.Common.Interface
{
    public interface ICacheService
    {
        T Get<T>(string cacheID, Func<T> getItemCallback, int secondsToCache);
        void Delete(string cacheID);

    }
}