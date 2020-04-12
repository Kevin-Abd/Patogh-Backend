namespace PatoghBackend.Contract
{
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.Tag;
	using PatoghBackend.Entity.Models;

	public interface ITagService : IService
	{
		Task<TagDorehami> AddTagToDorehami(AddTagToDorehami addTag);

		Task<List<Tag>> GetDorehamiTags(long dorehamiId);

		Task<Tag> GetTag(string tagNames);

		Task<List<Tag>> GetTags(List<string> tagNames);
	}
}