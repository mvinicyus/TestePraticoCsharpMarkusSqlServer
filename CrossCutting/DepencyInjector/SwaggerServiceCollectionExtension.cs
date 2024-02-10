using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CrossCutting.DepencyInjector
{
    public static class SwaggerServiceCollectionExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Plataforma - Pessoas", Version = "v1" });
            });
            return services;
        }
    }
}