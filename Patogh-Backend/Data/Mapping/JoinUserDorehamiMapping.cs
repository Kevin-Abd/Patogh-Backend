namespace PatoghBackend.Data.Mapping
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using PatoghBackend.Entity.Models;

	public class JoinUserDorehamiMapping : IEntityTypeConfiguration<JoinUserDorehami>
	{
		public void Configure(EntityTypeBuilder<JoinUserDorehami> entity)
		{
			entity
				.HasIndex(r => new { r.DorehamiId, r.UserId })
				.IsUnique();

			entity
				.HasOne(r => r.User)
				.WithMany(r => r.JoinedDorehamies)
				.HasForeignKey(r => r.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			entity
				.HasOne(r => r.Dorehami)
				.WithMany(r => r.JoinedUsers)
				.HasForeignKey(r => r.DorehamiId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}