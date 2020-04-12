namespace PatoghBackend.Data
{
	using Entity.Models;

	using Microsoft.EntityFrameworkCore;

	using PatoghBackend.Data.Mapping;

	public class MainDbContext : DbContext
	{
		public MainDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }

		public DbSet<Dorehami> Dorehamies { get; set; }

		public DbSet<GeoLocation> GeoLocations { get; set; }

		public DbSet<JoinUserDorehami> JoinUserDorehamies { get; set; }

		public DbSet<FavUserDorehami> FavUserDorehamies { get; set; }

		public DbSet<Image> Images { get; set; }

		public DbSet<ImageDorehami> ImageDorehamies { get; set; }

		public DbSet<Tag> Tags { get; set; }

		public DbSet<TagDorehami> TagDorehamies { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Configure default schema
			modelBuilder.HasDefaultSchema("Patogh");

			modelBuilder.ApplyConfiguration(new DorehamiMapping());
			modelBuilder.ApplyConfiguration(new UserMapping());
			modelBuilder.ApplyConfiguration(new GeoLocationMapping());
			modelBuilder.ApplyConfiguration(new JoinUserDorehamiMapping());
			modelBuilder.ApplyConfiguration(new FavUserDorehamiMapping());
			modelBuilder.ApplyConfiguration(new ImageMapping());
			modelBuilder.ApplyConfiguration(new ImageDorehamiMapping());
			modelBuilder.ApplyConfiguration(new TagDorehamiMapping());
			modelBuilder.ApplyConfiguration(new TagMapping());
		}
	}
}