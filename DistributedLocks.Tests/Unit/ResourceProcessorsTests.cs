using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SimpleLock.Processors;
using DistributedLock.Commons;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using SimpleLock.Extensions;
using FluentAssertions;

namespace DistributedLocks.Tests.Unit
{
    public class ResourceProcessorsTests
    {
        [Fact]
        public async Task DecoratedServicesShouldExecute()
        {
            //Given
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string>()
                    {
                        {"ConnectionStrings:Redis", "localhost, port:6379"},
                        {"RedLockConfiguration:AcquireWaitTime", "10"},
                        {"RedLockConfiguration:RetryTime", "3"},
                        {"RedLockConfiguration:ExpiryTime", "5"},
                    })
                .Build();

            services.AddLogging();
            services.AddRedisCache(config);
            services.AddTransient<IResourceProcessor, DatabaseResourceProcessor>();
            //services.Decorate<IResourceProcessor, SaveResourceProcessor>();
            services.Decorate<IResourceProcessor, RedLockResourceProcessor>();
            var provider = services.BuildServiceProvider();

            //When  
            var processor = provider.GetRequiredService<IResourceProcessor>();

            var resource = await processor.ProcessAsync(RedLockCreator.MyLockedResource);

            //Then
            resource.Name.Should().NotBeNull();
            
        }
    }
}