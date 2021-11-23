using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core;

namespace SimpleLock.Processors
{
    public class SaveResourceProcessor : IResourceProcessor
    {
        private readonly ILogger<SaveResourceProcessor> logger;
        private readonly IDistributedCache cache;
        private readonly IResourceProcessor next;
        private readonly ISerializer serializer;

        public SaveResourceProcessor(ILogger<SaveResourceProcessor> logger, IDistributedCache cache, IResourceProcessor next, ISerializer serializer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }
        
        public async Task<Resource> ProcessAsync(string resourceName)
        {
            var resource = new Resource("This is a created \"Resource\".", "\"Resource\"", DateTime.Now, "Hand");
            await cache.SetAsync(resourceName, serializer.Serialize(resource));

            return await next.ProcessAsync(resourceName);
        }
    }
}