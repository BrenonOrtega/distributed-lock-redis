using System.Threading;
using System.Threading.Tasks;
using DistributedLock.Commons;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleLock.Processors;

namespace SimpleLock
{
    internal class ResourceWorker : BackgroundService
    {
        private readonly ILogger<ResourceWorker> logger;
        private readonly IResourceProcessor processor;
        private readonly string resource;
        
        public ResourceWorker(ILogger<ResourceWorker> logger, IResourceProcessor processor)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.processor = processor ?? throw new System.ArgumentNullException(nameof(processor));
            this.resource = RedLockCreator.MyLockedResource;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                logger.LogInformation("Starting to process requested resource {resourceName}", resource);
                var requestedResource = await processor.ProcessAsync(resource);
                logger.LogInformation("Requested Resouce {requestedResouce}", requestedResource);

                await Task.Delay(1000);
            } while(stoppingToken.IsCancellationRequested is false);
        }
    }
}