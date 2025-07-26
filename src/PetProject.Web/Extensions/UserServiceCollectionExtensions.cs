using PetProject.Shared.Interfaces;
using PetProject.User.Services;
using PetProject.Web.Controllers;

namespace PetProject.Web.Extensions
{
    public static class UserServiceCollectionExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserController>();

            return services;
        }
    }
}
