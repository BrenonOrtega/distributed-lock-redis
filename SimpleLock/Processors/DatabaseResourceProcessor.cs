using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

namespace SimpleLock.Processors
{
    public class DatabaseResourceProcessor : IResourceProcessor
    {
        private readonly ILogger<DatabaseResourceProcessor> _logger;
        private readonly IDistributedCache _cache;
        private readonly ISerializer _serializer;

        public DatabaseResourceProcessor(ILogger<DatabaseResourceProcessor> logger, IDistributedCache cache, ISerializer serializer) =>
            (_cache, _logger, _serializer) = (cache?? throw new ArgumentNullException(nameof(cache)), logger, serializer);

        public async Task<Resource> ProcessAsync(string resourceName)
        {
            try
            {
                var resourceData = await _cache.GetAsync(resourceName);
                
                return _serializer.Deserialize<Resource>(resourceData);
            
            } catch(Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }
    }
}