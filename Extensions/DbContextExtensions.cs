using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPIIKcom.Extensions
{
	public static class DbContextExtensions
	{
		/// <summary>
		/// Checks if the databaseversion hsa the latests version of migration
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static bool AllMigrationsApplied(this DbContext context)
		{
			var applied = context.GetService<IHistoryRepository>()
				.GetAppliedMigrations()
				.Select(m => m.MigrationId);

			var total = context.GetService<IMigrationsAssembly>()
				.Migrations
				.Select(m => m.Key);

			return !total.Except(applied).Any();
		}
	}
}