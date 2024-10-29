using Microsoft.Extensions.Logging;
using System.Runtime.Caching;

namespace ConcreteIndustry.BLL.Services.Caching
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> logger;
        private readonly ObjectCache memoryCache;

        public CacheService(ILogger<CacheService> logger)
        {
            this.logger = logger;
            memoryCache = MemoryCache.Default;
        }

        public T GetData<T>(string key)
        {
            try
            {
                var data = (T)memoryCache.Get(key);
                if (data != null)
                {
                    logger.LogInformation($"Retrieved data from cache for key: {key}");
                }
                else
                {
                    logger.LogInformation($"No data found in cache for key: {key}");
                }
                return data;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error retrieving data from cache for key: {key}");
                throw;
            }
        }

        public object RemoveData(string key)
        {
            var res = true;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var result = memoryCache.Remove(key);
                }
                else
                {
                    res = false;
                }
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing data from cache for key: {key}");
                throw;
            }
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    memoryCache.Set(key, value, expirationTime);
                }
                else
                {
                    res = false;
                }
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error setting data to cache for key: {key}");
                throw;
            }
        }
    }
}
