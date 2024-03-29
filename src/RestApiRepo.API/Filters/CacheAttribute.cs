﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApiRepo.Cache;
using RestApiRepo.Configurations;
using RestApiRepo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Filters
{
    public class CacheAttribute : TypeFilterAttribute
    {
        public CacheAttribute(int timeToLive) : base(typeof(CacheAttributeImpl))
        {
            this.Arguments = new object[] { timeToLive };
        }
    }

    public class CacheAttributeImpl : IAsyncActionFilter
    {
        private readonly int _timeToLive;
        private readonly IResponseCacheService _responseCacheService;
        private readonly CacheConfiguration _redisConfiguration;
        private readonly ICacheKeyFromHttpRequestBuilder _cacheKeyBuilder;
        public CacheAttributeImpl(int timeToLive, IResponseCacheService responseCacheService, CacheConfiguration redisConfiguration)
        {
            _timeToLive = timeToLive;
            _responseCacheService = responseCacheService;
            _redisConfiguration = redisConfiguration;
            _cacheKeyBuilder = CacheKeyBuilder.CreateCacheKeyBuilder();
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!_redisConfiguration.Enabled)
            {
                await next();
                return;
            }

            var cacheKey = _cacheKeyBuilder
                .BuildKeyFromHttpRequest()
                .AddMethod(context.HttpContext.Request.Method)
                .AddPath(context.HttpContext.Request.Path)
                .AddQueryParameteres(context.HttpContext.Request.Query);
            var cachedResponse = await _responseCacheService
               .GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult { 
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                await _responseCacheService
                    .CacheResponseAsync(cacheKey,
                    okObjectResult.Value,
                    TimeSpan.FromSeconds(_timeToLive));
            }
        }
    }
}
