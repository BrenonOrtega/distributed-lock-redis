using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis.Extensions.Core;

namespace SimpleLock.Services
{
    public class SaveResourceService : IResourceService<Resource>
    {
        private readonly IDistributedCache cache;
        private readonly ISerializer serializer;

        public SaveResourceService(IDistributedCache cache, ISerializer serializer)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public Task ExecuteAsync(Resource resource)
        {
            var serializedData = serializer.Serialize(resource);

            return cache.SetAsync(resource.Name, serializedData);
        }
    }
}