namespace FoodFun.Web.Extensions
{
    using Core.AutoMapper;
    using Core.Contracts;
    using Core.Services;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Data;
    using Infrastructure.Data.Repositories;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDbContext<FoodFunDbContext>(options => options
                    .UseSqlServer(configuration.GetDefaultConnectionString()));

            return services;
        }

        public static IServiceCollection AddRedisCache(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.GetRedisConnectionString();
                    options.InstanceName = "RedisStore_";
                });

        public static IServiceCollection AddConfiguredSession(this IServiceCollection services)
            => services
                .AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromHours(10);
                    options.Cookie.IsEssential = true;
                });

        public static IServiceCollection AddDefaultIdentity(this IServiceCollection services)
        {
            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FoodFunDbContext>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
            => services
                .AddAutoMapper(typeof(AutoMapperServiceProfile), typeof(Program));

        public static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
                .AddScoped<IDishRepository, DishRepository>()
                .AddScoped<IDishCategoryRepository, DishCategoryRepository>();

        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddTransient<IProductService, ProductService>()
                .AddTransient<IProductCategoryService, ProductCategoryService>()
                .AddTransient<IDishService, DishService>()
                .AddTransient<IDishCategoryService, DishCategoryService>();

        public static IServiceCollection AddConfiguredCookies(this IServiceCollection services)
            => services
                .ConfigureApplicationCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(10);
                });

        public static IServiceCollection AddConfiguredControllersWithViews(this IServiceCollection services)
        {
            services
                .AddControllersWithViews(options =>
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });

            return services;
        }
    }
}
