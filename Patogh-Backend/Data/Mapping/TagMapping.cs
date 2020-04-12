namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class TagMapping : IEntityTypeConfiguration<Tag>
	{
		public void Configure(EntityTypeBuilder<Tag> entity)
		{
			entity.HasAlternateKey(r => r.Name);
		}
	}
}