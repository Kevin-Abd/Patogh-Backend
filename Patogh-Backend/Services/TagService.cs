namespace PatoghBackend.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Microsoft.EntityFrameworkCore;

	using PatoghBackend.Contract;
	using PatoghBackend.Data;
	using PatoghBackend.Dto.Tag;
	using PatoghBackend.Entity.Models;

	using Services.Common;

	public class TagService : Service, ITagService
	{
		private readonly MainDbContext dbContext;

		public TagService(MainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		private bool disposed = false; // To detect redundant calls

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					dbContext.Dispose();
					// TODO: dispose managed state (managed objects
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposed = true;
				base.Dispose(disposing);
			}
		}

		public async Task<TagDorehami> AddTagToDorehami(AddTagToDorehami addTag)
		{
			var tag = await dbContext.Tags
				.Where(r => r.Name == addTag.TagName)
				.FirstOrDefaultAsync();
			if (tag == null)
			{
				tag = new Tag
				{
					Name = addTag.TagName,
				};
				dbContext.Tags.Add(tag);
				await dbContext.SaveChangesAsync();
			}
			else
			{
				var alreadyExists = await dbContext.TagDorehamies
					.Where(r =>
							r.DorehamiId == addTag.DorehamiId &&
							r.TagId == tag.Id)
					.FirstOrDefaultAsync();
				if (alreadyExists != null)
				{
					return alreadyExists;
				}
			}

			var tagDorehami = new TagDorehami
			{
				DorehamiId = addTag.DorehamiId,
				TagId = tag.Id,
			};

			dbContext.TagDorehamies.Add(tagDorehami);
			await dbContext.SaveChangesAsync();
			return tagDorehami;
		}

		public async Task<List<Tag>> GetDorehamiTags(long dorehamiId)
		{
			var list = await dbContext.Tags
				.Where(r => r.TagDorehamies.Any(r => r.DorehamiId == dorehamiId))
				.ToListAsync();
			return list;
		}

		public async Task<Tag> GetTag(string tagName)
		{
			if (string.IsNullOrEmpty(tagName))
			{
				return null;
			}

			return await dbContext.Tags
				.Where(r => r.Name == tagName)
				.FirstOrDefaultAsync();
		}

		public async Task<List<Tag>> GetTags(List<string> tagNames)
		{
			var list = await dbContext.Tags
				.Where(r=> tagNames.Contains(r.Name))
				.ToListAsync();
			return list;
		}

	}
}