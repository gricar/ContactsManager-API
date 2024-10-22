using Contacts37.Application.Common.Behaviors;
using Contacts37.Domain.Specifications;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts37.Application.DependencyInjection
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationServicesRegistration).Assembly;

            services.AddAutoMapper(assembly);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddValidatorsFromAssembly(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IRegionValidator, RegionValidator>();

            return services;
        }
    }
}
