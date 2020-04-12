namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class FavUserDorehamiMapping : IEntityTypeConfiguration<FavUserDorehami>
	{
		public void Configure(EntityTypeBuilder<FavUserDorehami> entity)
		{
			entity
				.HasIndex(r => new { r.DorehamiId, r.UserId })
				.IsUnique();

			entity
				.HasOne(r => r.User)
				.WithMany(r => r.FavoriteDorehamies)
				.HasForeignKey(r => r.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			entity
				.HasOne(r => r.Dorehami)
				.WithMany(r=>r.FavoritedUsers)
				.HasForeignKey(r => r.DorehamiId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}