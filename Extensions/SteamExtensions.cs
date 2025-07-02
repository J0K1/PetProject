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

            services
                .AddHttpClient<SteamService>()                       
                .AddTypedClient((httpClient, sp) =>                    
                {
                    return new SteamService(steamApiKey, httpClient);
                });

            return services;
        }
    }
}
