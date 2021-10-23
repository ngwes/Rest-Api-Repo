using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rest_Api_Repo.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Filters
{
    public class ApiKeyAuthAttribute : TypeFilterAttribute
    {
        public ApiKeyAuthAttribute() : base(typeof(ApiKeyAuthAttributeImpl))
        {

        }
    }

    public class ApiKeyAuthAttributeImpl : IAsyncActionFilter
    {
        private readonly ApiKeySettings _apiKeySettings;
        private const string ApiKeyHeaderName = "ApiKey";

        public ApiKeyAuthAttributeImpl(ApiKeySettings apiKeySettings)
        {
            _apiKeySettings = apiKeySettings;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isApiKeySet = context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyFromRequest);
            if(!isApiKeySet)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if(!_apiKeySettings.Key.Equals(apiKeyFromRequest))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
