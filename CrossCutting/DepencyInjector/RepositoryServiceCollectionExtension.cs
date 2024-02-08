using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Context;

namespace CrossCutting.DepencyInjector
{
    public static class RepositoryServiceCollectionExtension
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(GenericRepository<,>));
            services.AddDbContext<DatabaseContext>(options =>
            {
                var connection = Environment.GetEnvironmentVariable("MySqlConnectionString");
                options
                .UseSqlServer(connection);
            });
            return services;
        }

        public static WebApplication UseRepositoryServices(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DatabaseContext>();
                _ = context.Database.EnsureCreated();
            }
            return app;
        }
    }
}