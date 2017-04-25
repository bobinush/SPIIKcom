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
	public class Seed
	{
		/// <summary>
		/// Ensures that there is testdata in the database.
		/// </summary>
		/// <param name="context"></param>
		public static async Task EnsureSeedData(IConfigurationRoot config, IServiceProvider serviceProvider)
		{
			string adminName = config["AdminEmail"];
			string adminPass = config["AdminPass"];
			string styrelseName = "rn222hk@student.lnu.se";
			string[] roles = { "Admin", "Styrelse", "Medlem" };
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			foreach (var role in roles)
			{
				var roleResult = await RoleManager.CreateAsync(new IdentityRole(role));
			}
			var adminUser = new ApplicationUser { UserName = adminName, Email = adminName };
			var _user = await UserManager.FindByEmailAsync(adminName);
			if (_user == null)
			{
				await UserManager.CreateAsync(adminUser, adminPass);
				// if (createPowerUser.Succeeded)
				await UserManager.AddToRoleAsync(adminUser, "Admin");
				
				var styrelseUser = new ApplicationUser { UserName = styrelseName, Email = styrelseName };
				await UserManager.CreateAsync(styrelseUser, adminPass);
				await UserManager.AddToRoleAsync(styrelseUser, "Styrelse");
			}

			// TODO : Seed with testdata.
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