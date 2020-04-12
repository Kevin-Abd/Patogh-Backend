namespace PatoghBackend
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Text;

	using Core;

	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.IdentityModel.Tokens;
	using Microsoft.OpenApi.Models;

	using PatoghBackend.Contract;
	using PatoghBackend.Data;
	using PatoghBackend.Services;

	public class Startup
	{
		private string swaggerVersion = "v3";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<MainDbContext>(
				options =>
				{
					options.UseSqlServer(ConfigLoader.GetConnectionString("MainDbConnectionString"));
					options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
				});

			services.AddDistributedMemoryCache();

			DiConfig(services);

			services.AddControllers();

			Authentication(services);

			Swagger(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(
					$"/swagger/{swaggerVersion}/swagger.json",
					$"Patogh API {swaggerVersion.ToUpper()}");
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private void DiConfig(IServiceCollection services)
		{
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IDorehamiService, DorehamiService>();
			services.AddScoped<ISmsService, SmsService>();
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<ITagService, TagService>();
			services.AddScoped<IDeleteService, DeleteService>();
		}

		private void Swagger(IServiceCollection services)
		{
			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description =
						"JWT Authorization header using the Bearer scheme. " +
						"\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header,
						},
						new List<string>()
					}
				});
				c.SwaggerDoc($"{swaggerVersion}", new OpenApiInfo
				{
					Version = $"{swaggerVersion}",
					Title = "Patogh API",
					Description = "Patogh API interface and communication",
					//TermsOfService = new Uri("https://example.com/terms"),
					//Contact = new OpenApiContact
					//{
					//	Name = "Shayne Boyer",
					//	Email = string.Empty,
					//	Url = new Uri("https://twitter.com/spboyer"),
					//},
					//License = new OpenApiLicense
					//{
					//	Name = "Use under LICX",
					//	Url = new Uri("https://example.com/license"),
					//}
				});

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
		}

		private void Authentication(IServiceCollection services)
		{
			// configure strongly typed settings objects

			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);

			// configure jwt authentication
			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
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
		}
	}
}