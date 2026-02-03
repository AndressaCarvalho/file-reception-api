using StackExchange.Redis;

namespace FileReception.Api.Filters.Cache.Helpers;

public static class CacheHelper
{
    public static async Task RemoveCacheAsync(Type controllerType, IDatabase redisDb, string? customPath = null, object? param = null)
    {
        var controllerName = controllerType.Name.Replace("Controller", "");

        var basePath = $"/api/{controllerName}";

        await redisDb.KeyDeleteAsync(basePath);
        await redisDb.KeyDeleteAsync(basePath.ToLower());

        if (!string.IsNullOrEmpty(customPath))
        {
            var listRoute = $"{basePath}/{customPath}";
            await redisDb.KeyDeleteAsync(listRoute);

            listRoute = $"{basePath.ToLower()}/{customPath}";
            await redisDb.KeyDeleteAsync(listRoute);

            await redisDb.KeyDeleteAsync(listRoute.ToLower());
        }

        if (param != null)
        {
            var itemRoute = $"{basePath}/{param}";

            await redisDb.KeyDeleteAsync(itemRoute);
            await redisDb.KeyDeleteAsync(itemRoute.ToLower());
        }
    }
}
