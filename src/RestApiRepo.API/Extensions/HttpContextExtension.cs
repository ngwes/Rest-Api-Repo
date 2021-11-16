using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Extensions
{
    public static class HttpContextExtension
    {
        public static string GetUserId(this HttpContext context)
        {
            if(context.User is null)
                return string.Empty;

            return context.User.Claims.Single(x => x.Type.Equals("id")).Value;
        }

    }
}
