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
	using PatoghBackend.Validation;
	using PatoghBackend.WebApi.Common;

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class DorehamiController : CustomApiController
	{
		private readonly IDorehamiService service;

		public DorehamiController(IDorehamiService dorehamiService)
		{
			this.service = dorehamiService;
		}

		/// <summary>
		/// create new Dorehami
		/// </summary>
		/// <response code="400">Could not create Dorehami</response>
		[HttpPost("createDorehami")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<ObjectDorehamiDetails>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
		public async Task<IActionResult> CreateDorehami([FromBody] RequestCreateDorehami request)
		{
			return await ExecuteAsync(
				() => service.Create(request, User),
				() => DorehamiValidation.ValidateCreateDorehami(request));
		}

		/// <summary>
		/// Upload  photo with intent to use in a Dorehami album
		/// </summary>
		/// <response code="200">Image uploaded</response>
		[HttpPost("uploadImage")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<IdStringWrapper>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
		public async Task<IActionResult> UploadImage([FromForm] FormFileWrapper fileWrapper)
		{
			return await ExecuteAsync(
				() => service.UploadImage(fileWrapper, User),
				() => General.ValiadateFormFileWrapper(fileWrapper));
		}

		/// <summary>
		/// get detailed list of all Dorehamies
		/// </summary>
		/// <response code="404">Dorehami not found</response>
		[HttpPost("getDetail")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<ObjectDorehamiDetails>))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
		public async Task<IActionResult> GetDetail([FromBody] IdStringWrapper wrapper)
		{
			return await ExecuteAsync(
				() => service.GetDetail(wrapper, User),
				() => General.CheckIsLong(wrapper.IdString));
		}

		/// <summary>
		/// get summery list of all Dorehamies
		/// </summary>
		[HttpGet("getSummery")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<List<ObjectDorehamiSummery>>))]
		public async Task<IActionResult> GetSummery()
		{
			return await ExecuteAsync(
				() => service.GetSummery(User));
		}

		/// <summary>
		/// searchers and returns list of summeries
		/// </summary>
		[HttpPost("search")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<List<ObjectDorehamiSummery>>))]
		public async Task<IActionResult> Search([FromBody] RequestSearchDorehami request)
		{
			// todo validations
			return await ExecuteAsync(
				() => service.Search(request, User));
		}

		/// <summary>
		/// Deletes the dorehami
		/// </summary>
		/// <response code="400">If user does not own the dorehami</response>
		[HttpPost("deleteDorehami")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
		public async Task<IActionResult> DeleteDorehami([FromBody] IdStringWrapper wrapper)
		{
			return await ExecuteAsync(
				() => service.DeleteDorehami(wrapper, User),
				() => General.CheckIsLong(wrapper.IdString));
		}
	}
}