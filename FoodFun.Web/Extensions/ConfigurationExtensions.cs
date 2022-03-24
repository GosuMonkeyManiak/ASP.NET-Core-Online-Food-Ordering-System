namespace FoodFun.Web.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");

        public static string GetRedisConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("Redis");
    }
}
