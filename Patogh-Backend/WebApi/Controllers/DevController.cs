namespace PatoghBackend.WebApi.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Mime;
	using System.Threading.Tasks;

	using Entity.Models;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Formatters;
	using Microsoft.EntityFrameworkCore;

	using PatoghBackend.Contract;
	using PatoghBackend.Data;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Dev;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.Image;

	using Validation;

	/// <summary>
	/// Developer controller.
	/// Used to Indirectly access the database
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	//[ApiExplorerSettings(IgnoreApi = true)]
	public class DevController : ControllerBase
	{
		private readonly MainDbContext dbContext;

		private readonly IImageService imageService;

		public DevController(MainDbContext dbContext, IImageService imageService)
		{
			this.dbContext = dbContext;
			this.imageService = imageService;
		}

		/// <summary>
		/// DEVELOPMENT ONLY
		/// get list of all users
		/// </summary>
		/// <param name="phoneNo">optional: to find a user by phone number</param>
		/// <response code="200">list of users</response>
		[HttpGet("getUsers")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<List<User>>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetUsers(string phoneNo)
		{
			if (string.IsNullOrEmpty(phoneNo))
			{
				var list = dbContext.Users.AsQueryable();
				return new OkObjectResult(list);
			}
			else
			{
				var pn = General.ConvertPhoneNoTo12Char("PhoneNo", phoneNo);
				var user = dbContext.Users.Where(r => r.PhoneNumber == pn).FirstOrDefault();

				if (user == null)
				{
					return new NotFoundResult();
				}
				return new OkObjectResult(new List<User>() { user });
			}
		}

		/// <summary>
		/// DEVELOPMENT ONLY
		/// get list of all dorehamies
		/// </summary>
		/// <response code="200">list of dorehamies</response>
		[HttpGet("getDorehamies")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseT<List<Dorehami>>))]
		public IActionResult GetDorehamies()
		{
			var list = dbContext.Dorehamies.AsQueryable();
			return new OkObjectResult(list);
		}

		[HttpPost("removeDorehami")]
		public IActionResult RemoveDorehami(ListStringWrapper wrapper)
		{
			foreach (var idS in wrapper.Values)
			{
				var id = long.Parse(idS);
				var dor = dbContext.Dorehamies
					.Where(r => r.Id == id)
					.FirstOrDefault();
				if (dor != null)
				{
					dbContext.Dorehamies.Remove(dor);
				}
			}
			dbContext.SaveChanges();
			return new OkResult();
		}

		[HttpPost("delete")]
		public IActionResult Delete([FromQuery] int m,[FromQuery] int id)
		{
			switch (m)
			{
				case 1:
					var js = dbContext.JoinUserDorehamies
						.Where(r => r.UserId == id)
						.ToList();
					var fs = dbContext.FavUserDorehamies
						.Where(r => r.UserId == id)
						.ToList();
					dbContext.RemoveRange(js);
					dbContext.RemoveRange(fs);
					dbContext.Users
						.Remove(new Entity.Models.User{Id = id});
					break;
				case 2:
					var imdr = dbContext.ImageDorehamies
						.Where(r => r.DorehamiId == id)
						.ToArray();
					dbContext.RemoveRange(imdr);
					dbContext.Dorehamies
						.Remove(new Entity.Models.Dorehami { Id = id });
					break;
				case 3:
					dbContext.Images
						.Remove(new Entity.Models.Image { Id = id });
					break;
				case 4:
					var t = dbContext.Tags
						.Where(r => r.Id == id)
						.FirstOrDefault();
					dbContext.Tags
						.Remove(t);
					break;
			}
			dbContext.SaveChanges();
			return Ok();
		}
	}
}