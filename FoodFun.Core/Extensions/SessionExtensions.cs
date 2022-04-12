namespace FoodFun.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Distributed;
    using System.Text.Json;

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static void Set<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            cache.SetString(key, JsonSerializer.Serialize(value), options);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static T? Get<T>(this IDistributedCache cache, string key)
        {
            var value = cache.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
