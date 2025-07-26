using PetProject.Shared.Interfaces;
using PetProject.Game.Services;
using PetProject.Web.Controllers;

namespace PetProject.Web.Extensions
{
    public static class GameServiceCollectionExtensions
    {
        public static IServiceCollection AddGameServices(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<GamesController>();

            return services;
        }
    }
}
