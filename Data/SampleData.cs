using System;
using System.Linq;
using SPIIKcom.Extensions;
using SPIIKcom.Models;
using SPIIKcom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using SPIIKcom.Enums;

namespace SPIIKcom.Models
{
	public class SampleData
	{
		/// <summary>
		/// Ensures that there is testdata in the database		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public static async Task Seed(IServiceProvider serviceProvider)
		{
			using (var serviceScope = serviceProvider.CreateScope())
			{
				var scopeServiceProvider = serviceScope.ServiceProvider;
				var db = scopeServiceProvider.GetService<ApplicationDbContext>();

				// Delete and create database
				// uncomment this below when adding new data to seed.
				//await db.Database.EnsureDeletedAsync();
				if (await db.Database.EnsureCreatedAsync())
				{
					await InsertTestDataAsync(scopeServiceProvider);
					await CreateAdminUserAsync(scopeServiceProvider);
				}
			}
		}
		private static async Task InsertTestDataAsync(IServiceProvider serviceProvider)
		{
			using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
				if (!context.MembershipTypes.Any())
				{
					context.MembershipTypes.AddRange(
						new MembershipType { Name = "1 år", Price = 100m, LengthInYears = 1 },
						new MembershipType { Name = "2 år", Price = 150m, LengthInYears = 2 }
					);
				}
				if (!context.Members.Any())
				{
					context.Members.AddRange(
						new Member { PersonalNumber = "20170101-0101", FirstName = "Kalle", LastName = "Anka", Email = "kalle@anka.se", JoinDate = DateTime.Today.AddDays(-1), ExpireDate = DateTime.Today.AddYears(1) },
						new Member { PersonalNumber = "20170102-0101", FirstName = "Kajsa", LastName = "Anka", Email = "kajsa@anka.se", JoinDate = DateTime.Today.AddDays(-2), ExpireDate = DateTime.Today.AddYears(1) },
						new Member { PersonalNumber = "20170103-0101", FirstName = "Musse", LastName = "Pigg", Email = "musse@pigg.se", JoinDate = DateTime.Today.AddDays(-3), ExpireDate = DateTime.Today.AddYears(1) }
					);
				}
				context.UnionMembers.AddRange(
					new UnionMember { Name = "Pernilla Johansson", Title = "Ordförande", UnionTypes = UnionTypeEnum.Styrelse, Email = "ordf@spiik.com", Quote = "Quote?", PictureSrc = "http://placehold.it/200x200" },
					new UnionMember { Name = "Didrik Fasth", Title = "Vice Ordförande Studiesocialt", UnionTypes = UnionTypeEnum.Styrelse, Email = "vordfss@spiik.com", Quote = "Quote?", PictureSrc = "http://placehold.it/200x200" },
					new UnionMember { Name = "Sofia Björkesjö", Title = "Vice Ordförande Utbildningsansvarig", UnionTypes = UnionTypeEnum.Styrelse, Email = "vordfuu@spiik.com", Quote = "Quote?", PictureSrc = "http://placehold.it/200x200" },
					new UnionMember { Name = "Robin Nowakowski", Title = "Kassör", UnionTypes = UnionTypeEnum.Styrelse, Email = "kassor@spiik.com", Quote = "Quote?", PictureSrc = "http://placehold.it/200x200" },
					new UnionMember { Name = "Simon Österdahl", Title = "Vice Kassör", UnionTypes = UnionTypeEnum.Styrelse | UnionTypeEnum.Sexmasteri, Email = "vkassor@spiik.com", Quote = "Quote?", PictureSrc = "http://placehold.it/200x200" },
					new UnionMember { Name = "Emma Lövgren", Title = "Sekreterare", UnionTypes = UnionTypeEnum.Styrelse, Email = "sekreterare@spiik.com", Quote = "Quote?", PictureSrc = "http://placehold.it/200x200" },
					new UnionMember { Name = "William Robertsson", Title = "Sexmästare", UnionTypes = UnionTypeEnum.Styrelse | UnionTypeEnum.Sexmasteri, Email = "sexmaster@spiik.com", Quote = "Quote?", PictureSrc = "http://placehold.it/200x200" }
				);
				await context.SaveChangesAsync();
			}
		}
		private static async Task CreateAdminUserAsync(IServiceProvider serviceProvider)
		{
			var env = serviceProvider.GetService<IHostingEnvironment>();

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddUserSecrets<Startup>();
			// For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709

			var configuration = builder.Build();
			var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
			string adminName = configuration["AdminEmail"];
			string adminPass = configuration["AdminPass"];
			// string[] roles = { "Admin", "Styrelse", "Sexmästeri", "Revisor", "Valberedningen", "Medlem" };
			string[] roles = { "Admin", "Styrelse" };
			string styrelseName = "rn222hk@student.lnu.se";

			var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
			foreach (UserTypeEnum role in Enum.GetValues(typeof(UserTypeEnum)))
			{
				if (!await roleManager.RoleExistsAsync(role.ToString()))
					await roleManager.CreateAsync(new IdentityRole(role.ToString()));
			}

			var adminUser = await userManager.FindByNameAsync(adminName);
			if (adminUser == null)
			{
				adminUser = new ApplicationUser { UserName = adminName, Email = adminName };
				await userManager.CreateAsync(adminUser, adminPass);
				await userManager.AddToRoleAsync(adminUser, UserTypeEnum.Admin.ToString());

				var styrelseUser = new ApplicationUser { UserName = styrelseName, Email = styrelseName };
				await userManager.CreateAsync(styrelseUser, adminPass);
				await userManager.AddToRoleAsync(styrelseUser, UserTypeEnum.Styrelse.ToString());
			}

		}
	}
}