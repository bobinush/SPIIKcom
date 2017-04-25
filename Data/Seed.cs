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

namespace SPIIKcom
{
	public static class Seed
	{

		/// <summary>
		/// Ensures that there is testdata in the database.
		/// </summary>
		/// <param name="context"></param>
		public static async Task EnsureSeedData(IConfigurationRoot config, IServiceProvider serviceProvider)
		{
			string adminName = config["AdminEmail"];
			string adminPass = config["AdminPass"];
			string adminRole = config["AdminRole"];

			var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			var roleResult = await RoleManager.CreateAsync(new IdentityRole(adminRole));
			var adminUser = new ApplicationUser { UserName = adminName, Email = adminName };
			var _user = await UserManager.FindByEmailAsync(adminName);
			if (_user == null)
			{
				var createPowerUser = await UserManager.CreateAsync(adminUser, adminPass);
				if (createPowerUser.Succeeded)
					await UserManager.AddToRoleAsync(adminUser, adminRole);
			}
			// if (context.AllMigrationsApplied())
			// {
			// 	if (!context.MembershipTypes.Any())
			// 	{
			// 		context.MembershipTypes.AddRange(
			// 			new MembershipType { Name = "Halvår", Price = 50m, LengthInYears = 0.5d },
			// 			new MembershipType { Name = "1 år", Price = 100m, LengthInYears = 1d },
			// 			new MembershipType { Name = "2 år", Price = 150m, LengthInYears = 2d }
			// 		);
			// 	}
			// 	if (!context.Members.Any())
			// 	{
			// 		context.Members.AddRange(
			// 			new Member { PersonalNumber = "20170101-0101", Name = "Kalle", LastName = "Anka", Email = "kalle@anka.se", JoinDate = DateTime.Today.AddDays(-1), ExpireDate = DateTime.Today.AddYears(1) },
			// 			new Member { PersonalNumber = "20170102-0101", Name = "Kajsa", LastName = "Anka", Email = "kajsa@anka.se", JoinDate = DateTime.Today.AddDays(-2), ExpireDate = DateTime.Today.AddYears(1) },
			// 			new Member { PersonalNumber = "20170103-0101", Name = "Musse", LastName = "Pigg", Email = "musse@pigg.se", JoinDate = DateTime.Today.AddDays(-3), ExpireDate = DateTime.Today.AddYears(1) }
			// 		);
			// 	}
			// 	context.SaveChanges();
			// }
		}
	}
}