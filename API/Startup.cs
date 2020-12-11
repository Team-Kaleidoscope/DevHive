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
using Data.Models.Classes;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Threading.Tasks;

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
				options.UseNpgsql(Configuration.GetConnectionString("DEV")));

			services.AddIdentity<User, Roles>()
				.AddEntityFrameworkStores<DevHiveContext>();

			services.Configure<IdentityOptions>(options =>
			{
				options.User.RequireUniqueEmail = true;

				options.Password.RequiredLength = 5;
			});

			// configure jwt authentication
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings").GetValue("Secret", ")H@McQfTB?E(H+Mb8x/A?D(Gr4u7x!A%WnZr4t7weThWmZq4KbPeShVm*G-KaPdSz%C*F-Ja6w9z$C&F"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        // var userId = int.Parse(context.Principal.Identity.Name);
                        // var user = userService.GetById(userId);
                        // if (user == null)
                        // {
                        //     // return unauthorized if user no longer exists
                        //     context.Fail("Unauthorized");
                        // }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
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
				//app.UseDeveloperExceptionPage();
				app.UseExceptionHandler("/api/Error"); //TESTING
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
