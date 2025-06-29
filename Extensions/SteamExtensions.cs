using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Services;

namespace PetProject.Extensions
{
    public static class SteamExtensions
    {
        public static IServiceCollection AddSteamAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            var steamApiKey = configuration["Steam:ApiKey"];

            // 1. Регистрируем SteamService как typed client:
            services
                .AddHttpClient<SteamService>()                       
                .AddTypedClient((httpClient, sp) =>                    
                {
                    // …читаем ключ из конфигурации и создаем SteamService
                    return new SteamService(steamApiKey, httpClient);
                });

            return services;
        }
    }
}
