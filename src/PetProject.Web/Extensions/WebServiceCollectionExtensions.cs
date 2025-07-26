using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using PetProject.Game.Extensions;
using PetProject.User.Extensions;
using PetProject.Web.Controllers;

namespace PetProject.Web.Extensions
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDatabaseServices(configuration)
                .AddGameServices()
                .AddUserServices()
                .AddSteamServices(configuration)
                .AddSharedServices();

            services.AddScoped<GamesController>();
            services.AddScoped<UserController>();

            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Error/Error404";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
                .SetApplicationName("PetProject");

            return services;
        }
    }
}
