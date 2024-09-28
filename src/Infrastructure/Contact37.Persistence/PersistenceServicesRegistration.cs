﻿using Contact37.Persistence.Repositories;
using Contacts37.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contact37.Persistence
{
	public static class PersistenceServicesRegistration
	{
		public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<Contact37DbContext>(options =>
			options.UseSqlServer(
				configuration.GetConnectionString("Contact37ConnectionString")));//#todo: Adicionar em AppSettings

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			return services;
		}
	}
}
