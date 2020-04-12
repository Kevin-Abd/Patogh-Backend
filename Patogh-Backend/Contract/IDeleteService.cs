namespace PatoghBackend.Contract
{
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;

	public interface IDeleteService : IService
	{
		Task User(long userId);

		Task Dorehami(long dorehamiId);

		Task Image(long imageId);

		Task Tag(long tagId);

		Task Tag(string tagName);
	}
}