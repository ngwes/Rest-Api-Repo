using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Cache
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string cacheKey); 
    }
}
