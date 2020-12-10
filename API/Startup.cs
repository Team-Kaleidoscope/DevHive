using System;
using API.Database;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Models.Classes;
using Models.DTOs;

namespace API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddDbContext<DevHiveContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("DEV")))
				.AddAuthentication()
				.AddJwtBearer();

			services.AddIdentity<User, Roles>()
				.AddEntityFrameworkStores<DevHiveContext>();
			services.AddAuthentication();

			services.Configure<IdentityOptions>(options =>
			{
				options.User.RequireUniqueEmail = true;

				// options.Password.RequiredLength = 5;
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
			});

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
			}
			else
			{
				app.UseExceptionHandler();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			IMapper configuration = new MapperConfiguration(cfg =>
			{
				//cfg.DestinationMemberNamingConvention = new ExactMatchNamingConvention();

				cfg.CreateMap<User, UserDTO>();
				cfg.CreateMap<UserDTO, User>();
				
			}).CreateMapper();
		}
}
}
