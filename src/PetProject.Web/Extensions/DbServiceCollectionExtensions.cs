using Microsoft.EntityFrameworkCore;
using PetProject.Shared.Data;

namespace PetProject.Web.Extensions
{
    public static class DbServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(AppDBContext)));
            });

            return services;
        }
    }
}
