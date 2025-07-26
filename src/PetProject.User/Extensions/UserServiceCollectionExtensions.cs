using PetProject.Shared.Interfaces;
using PetProject.User.Services;

namespace PetProject.User.Extensions
{
    public static class UserServiceCollectionExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
