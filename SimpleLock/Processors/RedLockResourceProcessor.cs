using System;
using System.Threading.Tasks;
using DistributedLock.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet;
using RedLockNet.SERedis;
using SimpleLock.Configuration;

namespace SimpleLock.Processors
{
    public class RedLockResourceProcessor : IResourceProcessor
    {
        private readonly ILogger<RedLockResourceProcessor> logger;
        private readonly RedLockFactory redLockFactory;
        private readonly RedLockConfiguration redLockConfig;
        private readonly IResourceProcessor next;

        public RedLockResourceProcessor(ILogger<RedLockResourceProcessor> logger, RedLockFactory redLockFactory, IResourceProcessor next, IOptions<RedLockConfiguration> redLockConfig)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.redLockFactory = redLockFactory ?? throw new ArgumentNullException(nameof(redLockFactory));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.redLockConfig = redLockConfig.Value ?? throw new ArgumentNullException(nameof(redLockConfig));
        }

        public async Task<Resource> ProcessAsync(string resourceName)
        {
            await using var redlock = await redLockFactory.CreateLockAsync(resourceName, redLockConfig.ExpiryTime, redLockConfig.AcquireWaitTime, redLockConfig.RetryTime);
            
            if(redlock.IsAcquired)
            {
                logger.LogInformation("RedLock Acquired, next processor is being called");
                return await next.ProcessAsync(resourceName);
            }

            throw new TimeoutException("Acquiring Lock Maximum retries exceeded");
        }
    }
}