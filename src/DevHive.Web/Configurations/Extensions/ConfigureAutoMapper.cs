using System;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureAutoMapper
	{
		public static void AutoMapperConfiguration(this IServiceCollection services)
		{
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}
	}
}