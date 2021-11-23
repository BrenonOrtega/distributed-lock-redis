using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace DistributedLock.Commons
{
    public static class RedLockCreator
    {
        private static string RedisConnectionString = "localhost,port: 6379";
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        public const string MyLockedResource = "my-locked-resource";
        private static TimeSpan ExpiringTime = TimeSpan.FromSeconds(5);

        private static List<RedLockEndPoint> _redlockEndpoints = new List<RedLockEndPoint>()
        {
            new DnsEndPoint("localhost", 6379),
        };

        public static RedLockFactory GetRedLockFactory()
        {
            return RedLockFactory.Create(_redlockEndpoints, new RedLockRetryConfiguration(5, 10));
        }
        
        public static Task<IRedLock> GetRedLockAsync()
        {
            var redLockFactory = GetRedLockFactory();
            
            return redLockFactory.CreateLockAsync(MyLockedResource, ExpiringTime);
        }
    }
}
