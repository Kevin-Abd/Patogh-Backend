namespace PatoghBackend.WebApi.Controllers
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	using PatoghBackend.Contract;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;
	using PatoghBackend.Validation;
	using PatoghBackend.WebApi.Common;

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : CustomApiController
	{
		private readonly IUserService service;

		public UserController(IUserService userService)
		{
			service = userService;
		}

		/// <summary>
		/// requests a new login token for the given phone number
		/// </summary>
		/// <param name="phoneNumber">phone number</param>
		/// <response code="200">Login Token geenrated</response>
		/// <response code="400">Invalid phone number (\\todo)</response>
		[AllowAnonymous]
		[HttpPost("requestLogin")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
		public async Task<IActionResult> RequestLoginToken([FromBody] PhoneNumberWrapper phoneNumber)
		{
			return await ExecuteAsync(
				() => service.RequestLoginToken(phoneNumber),
				() => UserValidation.ValidateAndNormalizePhoneNumber(phoneNumber));
		}

		/// <summary>
		/// login using phone number and login token
		/// </summary>
		/// <param name="request">request json</param>
		/// <response code="200">Session token if authentication was Successfull</response>
		/// <response code="403">If authentication fails</response>
		[AllowAnonymous]
		[HttpPost("authenticate")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<string>))]
		[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ApiResponse))]
		public async Task<IActionResult> Authenticate([FromBody] RequestUserLogin request)
		{
			return await ExecuteAsync(
				() => service.Authenticate(request),
				() => UserValidation.ValidateAndNormalizeAuthentication(request));
		}

		/// <summary>
		/// get the current user's details
		/// </summary>
		/// <response code="200">returns user's details</response>
		/// <response code="401">If authentication fails</response>
		[HttpPost("getUserDetails")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<ObjectUserDetails>))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetUserDetails()
		{
			return await ExecuteAsync(
				() => service.GetUserDetails(User));
		}

		/// <summary>
		/// replaces the users details with those provided.
		/// </summary>
		/// <param name="request"></param>
		/// <response code="200">returns new user's details</response>
		/// <response code="401">If authentication fails</response>
		[HttpPost("editUserDetails")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<ObjectUserDetails>))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> EditUserDetails([FromBody] ObjectUserDetails request)
		{
			return await ExecuteAsync(
				() => service.EditUserDetails(request, User),
				() => UserValidation.ValidateAndNormalizeUserDetails(request));
		}

		/// <summary>
		/// Deletes the current user profile
		/// </summary>
		/// <response code="200">returns new user's details</response>
		/// <response code="401">If authentication fails</response>
		[HttpPost("deleteUser")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> DeleteUser()
		{
			return await ExecuteAsync(
				() => service.DeleteUser(User));
		}

		/// <summary>
		/// replaces the users profile picture with the one provided
		/// </summary>
		/// <param name="fileWrapper">from Form</param>
		/// <response code="200">returns images idString</response>
		/// <response code="401">If authentication fails</response>
		[HttpPost("uploadProfilePicture")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<IdStringWrapper>))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse))]
		public async Task<IActionResult> UploadProfilePicture([FromForm] FormFileWrapper fileWrapper)
		{
			return await ExecuteAsync(
				() => service.UploadProfilePicture(fileWrapper, User),
				() => UserValidation.ValidateUplloadProfilePictuer(fileWrapper));
		}

		#region joinCRUD

		/// <summary>
		/// get list of all the dorehamies that the current user has joined
		/// </summary>
		/// <response code="200">returns use's joined dorehamies</response>
		/// <response code="400">If idString is not long</response>
		/// <response code="401">If authentication fails</response>
		[HttpPost("joinDorehamiGetSummery")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<List<ObjectDorehamiSummery>>))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> JoinDorehamiGetSummery()
		{
			return await ExecuteAsync(
				() => service.JoinDorehamiGetSummery(User));
		}

		/// <summary>
		/// Join the specified dorehamie
		/// </summary>
		/// <response code="200">If successfulls</response>
		/// <response code="400">If idString is not long</response>
		/// <response code="401">If authentication fails</response>
		/// <response code="404">If dorehami cannot be found</response>
		[HttpPost("joinDorehamiAdd")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> JoinDorehamiAdd(IdStringWrapper idString)
		{
			return await ExecuteAsync(
				() => service.JoinDorehamiAdd(idString, User),
				() => General.CheckIsLong(idString.IdString));
		}

		/// <summary>
		/// Leave a previously joined dorehami
		/// </summary>
		/// <response code="200">If successfulls</response>
		/// <response code="400">If idString is not long</response>
		/// <response code="401">If authentication fails</response>
		/// <response code="404">If dorehami cannot be found</response>
		[HttpPost("joinDorehamiRemove")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> JoinDorehamiRemove(IdStringWrapper idString)
		{
			return await ExecuteAsync(
				() => service.JoinDorehamiRemove(idString, User),
				() => General.CheckIsLong(idString.IdString));
		}

		#endregion joinCRUD

		#region favCRUD

		/// <summary>
		/// TODget list of all the dorehamies that the current user has favorited
		/// </summary>
		/// <response code="200">returns use's joined dorehamies</response>
		/// <response code="401">If authentication fails</response>
		[HttpPost("favDorehamiGetSummery")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<List<ObjectDorehamiSummery>>))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> FavDorehamiGetSummery()
		{
			return await ExecuteAsync(
				() => service.FavDorehamiGetSummery(User));
		}

		/// <summary>
		/// Favorite the specifed dorehami
		/// </summary>
		/// <response code="200">If successfulls</response>
		/// <response code="400">If idString is not long</response>
		/// <response code="401">If authentication fails</response>
		/// <response code="404">If dorehami cannot be found</response>
		[HttpPost("favDorehamiAdd")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> FavDorehamiAdd(IdStringWrapper idString)
		{
			return await ExecuteAsync(
				() => service.FavDorehamiAdd(idString, User),
				() => General.CheckIsLong(idString.IdString));
		}

		/// <summary>
		/// remove the previously sprecified dorehami
		/// </summary>
		/// <response code="200">If successfulls</response>
		/// <response code="400">If idString is not long</response>
		/// <response code="401">If authentication fails</response>
		/// <response code="404">If dorehami cannot be found</response>
		[HttpPost("favDorehamiRemove")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> FavDorehamiRemove(IdStringWrapper idString)
		{
			return await ExecuteAsync(
				() => service.FavDorehamiRemove(idString, User),
				() => General.CheckIsLong(idString.IdString));
		}

		#endregion favCRUD
	}
}