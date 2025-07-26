namespace PetProject.Web.Extensions
{
    public static class SharedServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("NotBanned", policy =>
                {
                    policy.RequireClaim("IsBanned", "false");
                });
            });

            return services;
        }
    }
}
