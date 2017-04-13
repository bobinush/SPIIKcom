using System.Linq;
using SPIIKcom.Extensions;
using SPIIKcom.Models;

namespace SPIIKcom.Data
{
	public static class ApplicationDbContextExtensions
	{
		public static void EnsureSeedData(this ApplicationDbContext context)
		{
			if (context.AllMigrationsApplied())
			{
				if (!context.MembershipTypes.Any())
				{
					context.MembershipTypes.AddRange(
						new MembershipType { Name = "1 termin", Price = 50m, LengthInYears = 0.5d },
						new MembershipType { Name = "1 år", Price = 100m, LengthInYears = 1d },
						new MembershipType { Name = "2 år", Price = 150m, LengthInYears = 2d }
					);
					context.SaveChanges();
				}
			}
		}
	}
}