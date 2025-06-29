using Microsoft.EntityFrameworkCore;
using PetProject.Services;
using PetProject.Controllers;

namespace PetProject.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();

            services.AddScoped<GameService>();
            services.AddScoped<GamesController>();

            services.AddScoped<UserService>();
            services.AddScoped<UserController>();

            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(AppDBContext)));
            });
            
            return services;
        }
    }
}
