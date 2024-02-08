using Application.Boudary.Person;
using Application.Command.Person;
using Application.Handler.Person;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.DepencyInjector
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<CreatePersonCommand, CreatePersonOutput>, CreatePersonHandler>();
            services.AddTransient<IRequestHandler<UpdatePersonCommand, UpdatePersonOutput>, UpdatePersonHandler>();
            services.AddTransient<IRequestHandler<DeletePersonCommand, DeletePersonOutput>, DeletePersonHandler>();
            services.AddTransient<IRequestHandler<GetPersonsCommand, GetPersonsOutput>, GetPersonsHandler>();
            services.AddTransient<IRequestHandler<GetPersonCommand, GetPersonOutput>, GetPersonHandler>();
            return services;
        }
    }
}