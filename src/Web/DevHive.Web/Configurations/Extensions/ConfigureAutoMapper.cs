using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureAutoMapper
	{
		public static void AutoMapperConfiguration(this IServiceCollection services)
		{
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}

		public static void UseAutoMapperConfiguration(this IApplicationBuilder app)
		{
			_ = new MapperConfiguration(cfg =>
			{
				cfg.AllowNullCollections = true;
			});
		}
	}
}
