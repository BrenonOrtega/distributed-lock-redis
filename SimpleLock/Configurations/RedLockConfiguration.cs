using System;

namespace SimpleLock.Configuration
{
    public class RedLockConfiguration
    {
        private int acquireWaitTime;
        private int retryTime;
        private int expiryTime;

        public TimeSpan AcquireWaitTime { get => TimeSpan.FromSeconds(acquireWaitTime); set => acquireWaitTime = value.Seconds; }
        public TimeSpan RetryTime {get => TimeSpan.FromSeconds(retryTime); set => retryTime = (int)value.Seconds; }
        public TimeSpan ExpiryTime { get => TimeSpan.FromSeconds(expiryTime); set => expiryTime = (int)value.Seconds; }
    }
}