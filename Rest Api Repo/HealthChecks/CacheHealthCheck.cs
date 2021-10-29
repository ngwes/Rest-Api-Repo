using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rest_Api_Repo.HealthChecks
{
    public class CacheHealthCheck : IHealthCheck
    {
        private readonly IDistributedCache _distributedCache;

        public CacheHealthCheck(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                _distributedCache.GetString("health");
                return Task.FromResult(
                HealthCheckResult.Healthy("cache is healthy"));
            }
            catch (Exception)
            {

                return Task.FromResult(
                HealthCheckResult.Unhealthy("cache is unhealthy"));
            }
        }
    }
}
