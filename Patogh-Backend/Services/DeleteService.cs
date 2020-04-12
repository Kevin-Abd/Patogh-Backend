namespace PatoghBackend.Services
{
	using System.Linq;
	using System.Threading.Tasks;

	using Microsoft.EntityFrameworkCore;

	using PatoghBackend.Contract;
	using PatoghBackend.Data;
	using PatoghBackend.Services.Common;

	public class DeleteService : Service, IDeleteService
	{
		private readonly MainDbContext dbContext;

		public DeleteService(MainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task User(long userId)
		{
			var joins = await dbContext.JoinUserDorehamies
				.Where(r => r.UserId == userId)
				.ToListAsync();
			var favs = await dbContext.FavUserDorehamies
				.Where(r => r.UserId == userId)
				.ToListAsync();
			dbContext.RemoveRange(joins);
			dbContext.RemoveRange(favs);
			dbContext.Users
				.Remove(new Entity.Models.User { Id = userId });

			await dbContext.SaveChangesAsync();
		}

		public async Task Dorehami(long dorehamiId)
		{
			var imageDorehamies = dbContext.ImageDorehamies
						.Where(r => r.DorehamiId == dorehamiId)
						.ToArray();
			dbContext.RemoveRange(imageDorehamies);
			dbContext.Dorehamies
				.Remove(new Entity.Models.Dorehami { Id = dorehamiId });

			await dbContext.SaveChangesAsync();
		}

		public async Task Image(long imageId)
		{
			dbContext.Images
				.Remove(new Entity.Models.Image { Id = imageId });

			await dbContext.SaveChangesAsync();
		}

		public async Task Tag(long tagId)
		{
			var t = await dbContext.Tags
						.Where(r => r.Id == tagId)
						.FirstOrDefaultAsync();
			dbContext.Tags.Remove(t);

			await dbContext.SaveChangesAsync();
		}

		public async Task Tag(string tagName)
		{
			var t = await dbContext.Tags
						.Where(r => r.Name == tagName)
						.FirstOrDefaultAsync();
			dbContext.Tags.Remove(t);

			await dbContext.SaveChangesAsync();
		}

		private bool disposed = false; // To detect redundant calls

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
				}

				disposed = true;
				base.Dispose(disposing);
			}
		}
	}
}