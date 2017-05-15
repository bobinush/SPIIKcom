using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SPIIKcom.Data;
using SPIIKcom.Models;
using SPIIKcom.Services;

namespace SPIIKcom
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			// if (env.IsDevelopment())
			// {
			// 	// For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
			// 	builder.AddUserSecrets<Startup>();
			// }

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();

		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Make the configuration available in the app globally through Dependency Injection
			// https://joonasw.net/view/asp-net-core-1-configuration-deep-dive
			services.AddSingleton<IConfiguration>(Configuration);

			// Add framework services.
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

			// services.AddEntityFramework()
			// 	.AddEntityFrameworkSqlServer()
			// 	.AddDbContext<ApplicationDbContext>(options =>
			// 		options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>(x =>
				{
					x.Password.RequiredLength = 6;
					x.Password.RequireUppercase = false;
					x.Password.RequireLowercase = false;
					x.Password.RequireNonAlphanumeric = false;
					x.Password.RequireDigit = false;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddMemoryCache();
			services.AddSession();
			// services.AddDistributedMemoryCache();
			services.AddMvc(options =>
			{
				options.ModelBinderProviders.Insert(0, new FlagsEnumBinderProvider());
			});
			services.AddRouting(options => options.LowercaseUrls = true);


			// Add application services.
			services.AddTransient<IEmailSender, AuthMessageSender>();
			services.AddTransient<ISmsSender, AuthMessageSender>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseIdentity();
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			// Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
			app.UseSession();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "areaRoute",
					template: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
					);
				routes.MapRoute(
					name: "Root",
					template: "{action}",
					defaults: new { controller = "Home", action = "Index" }
				);
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});


			// Apply all migrations and seed database with testdata
			// using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			// {
			// 	var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
			// 	context.Database.Migrate();
			SampleData.Seed(app.ApplicationServices).Wait();
			// }
		}
	}
}
