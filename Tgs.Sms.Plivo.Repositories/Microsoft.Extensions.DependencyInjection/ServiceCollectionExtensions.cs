using System;
using Microsoft.EntityFrameworkCore;
using Tgs.Sms.Plivo;
using Tgs.Sms.Plivo.Repositories;
using Tgs.Sms.Plivo.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPlivoRepositories(
			this IServiceCollection services,
			Action<IServiceProvider, DbContextOptionsBuilder> dbContextConfigure)
			=> services
				.AddDbContext<TgsContext>(dbContextConfigure)
				.AddTransient<ITgsRepository, TgsRepository>();
	}
}
