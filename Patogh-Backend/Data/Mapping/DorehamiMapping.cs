namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class DorehamiMapping : IEntityTypeConfiguration<Dorehami>
	{
		public void Configure(EntityTypeBuilder<Dorehami> entity)
		{
			entity
				.HasOne(r => r.Location)
				.WithOne(r => r.Dorehami)
				.HasForeignKey<GeoLocation>(r => r.DorehamiId);

			entity
				.HasOne(r => r.Thumbnail)
				.WithMany()
				.HasForeignKey(r => r.ThumbnailId);

			entity
				.HasOne(r => r.Creator)
				.WithMany()
				.HasForeignKey(r => r.CreatorId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}