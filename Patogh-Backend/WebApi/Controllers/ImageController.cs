namespace PatoghBackend.WebApi.Controllers
{
	using System;
	using System.Net.Mime;
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	using PatoghBackend.Contract;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Validation;
	using PatoghBackend.WebApi.Common;

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : CustomApiController
	{
		private readonly IImageService service;

		public ImageController(IImageService imageService)
		{
			service = imageService;
		}

		/// <summary>
		/// download the complete image file (png)
		/// </summary>
		[AllowAnonymous]
		[HttpPost("downloadImage")]
		[Produces("image/png", "application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DownloadImage([FromBody] IdStringWrapper wrapper)
		{
			try
			{
				General.CheckIsLong(wrapper.IdString);
				var stream = await service.DownloadImage(wrapper);
				stream.Position = 0;
				return File(stream, "image/png");
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		/// <summary>
		/// download a square thumbnail (128x128)
		/// </summary>
		[AllowAnonymous]
		[HttpPost("downloadThumbnail")]
		[Produces("image/jpg", "application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DownloadThumbnail([FromBody] IdStringWrapper wrapper)
		{
			try
			{
				General.CheckIsLong(wrapper.IdString);
				var stream = await service.DownloadThumbnail(wrapper);
				stream.Position = 0;
				return File(stream, MediaTypeNames.Image.Jpeg);
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}
	}
}