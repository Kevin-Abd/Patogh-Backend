namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class GeoLocationMapping : IEntityTypeConfiguration<GeoLocation>
	{
		public void Configure(EntityTypeBuilder<GeoLocation> entity)
		{
			entity.Property(r => r.Latitude).HasColumnType("decimal(13,10)");
			entity.Property(r => r.Longitude).HasColumnType("decimal(13,10)");
			entity
				.HasOne(r => r.Dorehami)
				.WithOne(r => r.Location)
				.HasForeignKey<GeoLocation>(r => r.DorehamiId)
				.IsRequired();
			//entity.Ignore(r => r.Id);
			//entity.HasKey(r => r.DorehamiId);

			//entity
			//	.HasRequired(r => r.Dorehami)
			//	.WithOptional(r => r.Location);
		}
	}
}