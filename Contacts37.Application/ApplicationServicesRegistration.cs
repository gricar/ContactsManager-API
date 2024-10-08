using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contacts37.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            //Automapper - Dependency Injection
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //MediatR - Dependency Injection
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
