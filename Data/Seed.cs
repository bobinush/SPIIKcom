using System;
using System.Linq;
using SPIIKcom.Extensions;
using SPIIKcom.Models;

namespace SPIIKcom.Data
{
	public static class ApplicationDbContextExtensions
	{
		/// <summary>
		/// Ensures that there is testdata in the database.
		/// </summary>
		/// <param name="context"></param>
		public static void EnsureSeedData(this ApplicationDbContext context)
		{
			if (context.AllMigrationsApplied())
			{
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
						new Member { PersonalNumber = "20170101-0101", Name = "Kalle", LastName="Anka", Email = "kalle@anka.se", JoinDate = DateTime.Today.AddDays(-1), ExpireDate = DateTime.Today.AddYears(1) },
						new Member { PersonalNumber = "20170102-0101", Name = "Kajsa", LastName="Anka", Email = "kajsa@anka.se", JoinDate = DateTime.Today.AddDays(-2), ExpireDate = DateTime.Today.AddYears(1) },
						new Member { PersonalNumber = "20170103-0101", Name = "Musse", LastName="Pigg", Email = "musse@pigg.se", JoinDate = DateTime.Today.AddDays(-3), ExpireDate = DateTime.Today.AddYears(1) }
					);
				}
				context.SaveChanges();
			}
		}
	}
}