using PetProject.Shared.Interfaces;
using PetProject.Game.Services;

namespace PetProject.Game.Extensions
{
    public static class GameServiceCollectionExtensions
    {
        public static IServiceCollection AddGameServices(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();

            return services;
        }
    }
}
