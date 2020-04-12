namespace PatoghBackend.Contract
{
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Dorehami;

	public interface IDorehamiService : IService
	{
		Task<ObjectDorehamiDetails> Create(RequestCreateDorehami request, ClaimsPrincipal actingUser);

		Task<IdStringWrapper> UploadImage(FormFileWrapper request, ClaimsPrincipal actingUser);

		//Task<List<ObjectDorehamiDetails>> GetDetails(ClaimsPrincipal actingUser);

		Task<ObjectDorehamiDetails> GetDetail(IdStringWrapper wrapper, ClaimsPrincipal actingUser);

		Task<List<ObjectDorehamiSummery>> GetSummery(ClaimsPrincipal actingUser);

		Task<List<ObjectDorehamiSummery>> Search(RequestSearchDorehami request, ClaimsPrincipal actingUser);

		Task DeleteDorehami(IdStringWrapper wrapper, ClaimsPrincipal actingUser);

		//Task<List<ObjectDorehamiSummery>> GetSummery(List<string> ids);

		//TODO search filter...
	}
}