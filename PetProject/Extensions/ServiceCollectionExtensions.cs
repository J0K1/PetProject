using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using PetProject.Controllers;
using PetProject.Services;
using PetProject.Services.Interfaces;

namespace PetProject.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<GamesController>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserController>();

            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(AppDBContext)));
            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("NotBanned", policy =>
                {
                    policy.RequireClaim("IsBanned", "false");
                });
            });

            return services;
        }
    }
}
