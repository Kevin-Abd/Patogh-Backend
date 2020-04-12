namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class ImageDorehamiMapping : IEntityTypeConfiguration<ImageDorehami>
	{
		public void Configure(EntityTypeBuilder<ImageDorehami> entity)
		{
			entity
				.HasIndex(r => new { r.DorehamiId, r.ImageId })
				.IsUnique();
			entity
				.HasOne(r => r.Image)
				.WithMany()
				.HasForeignKey(r => r.ImageId)
				.OnDelete(DeleteBehavior.Cascade);

			entity
				.HasOne(r => r.Dorehami)
				.WithMany(r => r.Images)
				.HasForeignKey(r => r.DorehamiId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}