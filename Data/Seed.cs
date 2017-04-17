using System;
using System.Linq;
using SPIIKcom.Extensions;
using SPIIKcom.Models;
using SPIIKcom;
using Microsoft.Extensions.Configuration;

namespace SPIIKcom.Data
{
	public static class ApplicationDbContextExtensions
	{
		/// <summary>
		/// Ensures that there is testdata in the database.
		/// </summary>
		/// <param name="context"></param>
		public static void EnsureSeedData(this ApplicationDbContext context, IConfigurationRoot configuration)
		{
			if (context.AllMigrationsApplied())
			{
				string adminName = configuration.GetSection("AdminEmail").Value;
				string adminPass = configuration.GetSection("AdminPass").Value;
				string roleName = configuration.GetSection("AdminRole").Value;
				if (!context.Users.Where(x => x.Email == adminName).Any())
				{
					// TODO : Add admin login
					// var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
					// var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
					// //Create Role Admin if it does not exist
					// role = new IdentityRole(roleName);
					// var roleresult = roleManager.Create(role);
					// // Create User Admin if it does not exist
					// adminUser = new ApplicationUser { UserName = adminName, Email = adminName };
					// var result = userManager.Create(adminUser, adminPass);
					// result = userManager.SetLockoutEnabled(adminUser.Id, false);
					// // Add user admin to Role Admin if not already added
					// result = userManager.AddToRole(adminUser.Id, role.Name);
				}

				if (!context.MembershipTypes.Any())
				{
					context.MembershipTypes.AddRange(
						new MembershipType { Name = "Halvår", Price = 50m, LengthInYears = 0.5d },
						new MembershipType { Name = "1 år", Price = 100m, LengthInYears = 1d },
						new MembershipType { Name = "2 år", Price = 150m, LengthInYears = 2d }
					);
				}
				if (!context.Members.Any())
				{
					context.Members.AddRange(
						new Member { PersonalNumber = "20170101-0101", Name = "Kalle", LastName = "Anka", Email = "kalle@anka.se", JoinDate = DateTime.Today.AddDays(-1), ExpireDate = DateTime.Today.AddYears(1) },
						new Member { PersonalNumber = "20170102-0101", Name = "Kajsa", LastName = "Anka", Email = "kajsa@anka.se", JoinDate = DateTime.Today.AddDays(-2), ExpireDate = DateTime.Today.AddYears(1) },
						new Member { PersonalNumber = "20170103-0101", Name = "Musse", LastName = "Pigg", Email = "musse@pigg.se", JoinDate = DateTime.Today.AddDays(-3), ExpireDate = DateTime.Today.AddYears(1) }
					);
				}
				context.SaveChanges();
			}
		}
	}
}