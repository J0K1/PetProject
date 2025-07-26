using PetProject.Steam.Services;

namespace PetProject.Web.Extensions
{
    public static class SteamServiceCollectionExtensions
    {
        public static IServiceCollection AddSteamServices(this IServiceCollection services, IConfiguration configuration)
        {
            string steamApiKey = configuration["Steam:ApiKey"]!;

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
