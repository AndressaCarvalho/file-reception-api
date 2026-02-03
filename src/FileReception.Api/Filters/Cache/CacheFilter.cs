using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using StackExchange.Redis;

namespace FileReception.Api.Filters.Cache;

public class CacheFilter : IAsyncActionFilter
{
    private readonly IDatabase _redisDb;

    public CacheFilter(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var key = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

        var cached = await _redisDb.StringGetAsync(key);
        if (!cached.IsNullOrEmpty)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ContentResult
            {
                Content = cached,
                ContentType = "application/json",
                StatusCode = 200
            };

            return;
        }

        var executedContext = await next();

        if (executedContext.Result is Microsoft.AspNetCore.Mvc.ObjectResult objectResult)
        {
            var json = JsonSerializer.Serialize(objectResult.Value);
            await _redisDb.StringSetAsync(key, json, TimeSpan.FromMinutes(10));
        }
    }
}
