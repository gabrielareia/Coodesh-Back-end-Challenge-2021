using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        const string APIKEY = "apiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEY, out var incomingApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "An Api Key is needed to perform this action."
                };
                return;
            }

            IConfiguration settings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            string apiKey = settings.GetValue<string>(APIKEY);

            if (!apiKey.Equals(incomingApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "The Api Key provided is not valid."
                };
                return;
            }

            await next();
        }
    }
}
