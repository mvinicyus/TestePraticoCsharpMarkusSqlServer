using Infrastructure.Message;
using Infrastructure.Message.Interface;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.DepencyInjector
{
    public static class InfrastructureServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMessagesHandler, MessagesHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            return services;
        }
    }
}