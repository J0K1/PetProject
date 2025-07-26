using Microsoft.EntityFrameworkCore;
using PetProject.Game.Data;
using PetProject.User.Data;

namespace PetProject.Web.Extensions
{
    public static class DbServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(UserDbContext)));
            });

            services.AddDbContext<GameDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(GameDbContext)));
            });

            return services;
        }
    }
}
