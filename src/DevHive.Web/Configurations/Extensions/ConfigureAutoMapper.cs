using System;
using AutoMapper;
//using AutoMapper.Configuration;
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
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AllowNullCollections = true;
			});
		}
	}
}