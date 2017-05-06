using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPIIKcom.Models;

namespace SPIIKcom.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
			//this.Configuration.LazyLoadingEnabled = true;
			//this.Configuration.ProxyCreationEnabled = false;
		}

		public DbSet<Member> Members { get; set; }
		public DbSet<MembershipType> MembershipTypes { get; set; }
		public DbSet<StaticPage> StaticPages { get; set; }
		public DbSet<BoardMember> BoardMembers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);

			base.OnModelCreating(modelBuilder);
			//Database.SetInitializer<ApplicationDbContext>(null);   // Annars fick man error på att databasen har ändrats etc.
			// modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();  // För att den ska leta efter Adress och inte Adress(s)
			// modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
		}

		// static ApplicationDbContext()
		// {
		// 	// Set the database intializer which is run once during application start
		// 	// This seeds the database with admin user credentials and admin role
		// 	// Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
		// 	//Database.Initialize(true);
		// 	//Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrationsx.Configurationx>());
		// 	//Database.SetInitializer(new ApplicationDbInitializer());
		// }

		// public static ApplicationDbContext Create()
		// {
		// 	return new ApplicationDbContext();
		// }
	}
}
