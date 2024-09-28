using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contacts37.Application
{
	public static class ApplicationServicesRegistration
	{
		public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
		{
			//Automapper - Injeção de Dependência
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			//MediatR - Injeção de Dependência
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
			return services;
		}
	}
}
