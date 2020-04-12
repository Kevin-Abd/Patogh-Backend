namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class TagDorehamiMapping : IEntityTypeConfiguration<TagDorehami>
	{
		public void Configure(EntityTypeBuilder<TagDorehami> entity)
		{
			entity
				.HasIndex(r => new { r.DorehamiId, r.TagId })
				.IsUnique();

			entity
				.HasOne(r => r.Tag)
				.WithMany(r => r.TagDorehamies)
				.HasForeignKey(r => r.TagId)
				.OnDelete(DeleteBehavior.Cascade);

			entity
				.HasOne(r => r.Dorehami)
				.WithMany(r => r.TagDorehamies)
				.HasForeignKey(r => r.DorehamiId)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}