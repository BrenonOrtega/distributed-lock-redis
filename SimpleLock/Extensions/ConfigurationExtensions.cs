using System;
using DistributedLock.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLock.Configuration;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace SimpleLock.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration config) =>
            /// <Summary>First - We Add the redlock configuration,
            /// It consists of three simple params, <param see="ExpiryTime"/> the Lock Expire time, that is the time that the resource will 
            /// be locked without a "heartbeat" telling that the resource is still in use.
            /// The <Param>AwaitTime</Param> is the time that the RedLock will await to acquire the lock, after this time it will give up.
            /// The RetryTime ?????
            /// </Summary>
            services.Configure<RedLockConfiguration>(config.GetSection(nameof(RedLockConfiguration)))

                /// <Summary>
                /// This is the method that configures the extensions to serialize, deserialize data from bytes to objects
                // with redis singularities in mind, enables us to use the IDistributedCache and IDatabase abstracting the hard work of working with byte arrays.
                /// </Summary>
                .AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(new RedisConfiguration()
                {
                    ConnectionString = config.GetConnectionString(nameof(StackExchange.Redis)),
                })

                /// Personal class that creates an IRedLockFactory.
                .AddSingleton(_ => RedLockCreator.GetRedLockFactory())

                /// Adds IDatabase to ServiceProvider
                .AddSingleton(_ => ConnectionMultiplexer.Connect(config.GetConnectionString(nameof(StackExchange.Redis))).GetDatabase())

                /// Adds IDistributedCache implementation to Service Provider.
                .AddDistributedRedisCache(x => x.Configuration = config.GetConnectionString(nameof(StackExchange.Redis)))
                ;
    }
}
