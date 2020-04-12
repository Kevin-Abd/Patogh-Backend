namespace PatoghBackend.Contract
{
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;

	public interface IUserService : IService
	{
		/// <summary>
		/// makes a request to issue a new login token.
		/// creats a new user if the phone number is new
		/// </summary>
		/// <param name="phoneNumber"></param>
		Task RequestLoginToken(PhoneNumberWrapper phoneNumber);

		//input mobile number and token, output session token/error
		Task<StringWrapper> Authenticate(RequestUserLogin request);

		Task<ObjectUserDetails> GetUserDetails(ClaimsPrincipal actingUser);

		Task<ObjectUserDetails> EditUserDetails(ObjectUserDetails request, ClaimsPrincipal actingUser);

		Task DeleteUser(ClaimsPrincipal actingUser);

		Task<IdStringWrapper> UploadProfilePicture(FormFileWrapper request, ClaimsPrincipal actingUser);

		Task JoinDorehamiAdd(IdStringWrapper idString, ClaimsPrincipal actingUser);

		Task JoinDorehamiRemove(IdStringWrapper idString, ClaimsPrincipal actingUser);

		Task<List<ObjectDorehamiSummery>> JoinDorehamiGetSummery(ClaimsPrincipal actingUser);

		Task FavDorehamiAdd(IdStringWrapper idString, ClaimsPrincipal actingUser);

		Task FavDorehamiRemove(IdStringWrapper idString, ClaimsPrincipal actingUser);

		Task<List<ObjectDorehamiSummery>> FavDorehamiGetSummery(ClaimsPrincipal actingUser);
	}
}