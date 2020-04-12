namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class UserMapping : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> entity)
		{
			entity
				.HasOne(r => r.ProfilePicture)
				.WithOne(r => r.User)
				.HasForeignKey<Image>(r => r.UserId);
		}
	}
}