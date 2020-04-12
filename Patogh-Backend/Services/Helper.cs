namespace PatoghBackend.Services
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Http;
	using Microsoft.EntityFrameworkCore;

	using PatoghBackend.Data;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;
	using PatoghBackend.Entity.Models;
	using PatoghBackend.Exceptions.Common;
	using PatoghBackend.Exceptions.General;

	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.Formats;
	using SixLabors.ImageSharp.PixelFormats;

	public static class Helper
	{
		internal static async Task<User> GetUserByClaim(MainDbContext dbContext, ClaimsPrincipal actingUser, bool doTracking = false)
		{
			if (actingUser == null)
			{
				throw new ArgumentNullException(nameof(actingUser));
			}

			var id = actingUser.FindFirst(ClaimTypes.Name)?.Value;
			if (id == null)
			{
				throw new NullReferenceException("token with no claim");
			}

			var idlong = long.Parse(id);

			var q0 = dbContext.Users.AsQueryable();
			IQueryable<User> q1;

			if (doTracking)
			{
				q1 = q0.AsTracking();
			}
			else
			{
				q1 = q0.AsNoTracking();
			}

			var user = await q1.Where(r => r.Id == idlong)
							  .FirstOrDefaultAsync();
			if (user == null)
			{
				throw new RecordNotFoundException(nameof(User));
			}

			return user;
		}

		internal static Expression<Func<Dorehami, ObjectDorehamiDetails>> DetailFromDorehami(long? userId)
		{
			return r => new ObjectDorehamiDetails
			{
				Id = r.Id.ToString(),
				Name = r.Name,
				CreatorId = r.CreatorId.ToString(),
				StartTime = r.StartTime,
				EndTime = r.EndTime,
				DateCreated = r.DateCreated,

				Description = r.Description,
				Summery = r.Summery,
				Category = r.Category,
				Size = r.Size,
				RemainingSize = r.JoinedUsers != null ? r.Size - r.JoinedUsers.Count :-1,

				IsPhysical = r.IsPhysical,
				Address = r.IsPhysical ? r.Address : string.Empty,
				Province = r.IsPhysical ? r.Province : string.Empty,
				Latitude = r.IsPhysical ? r.Location.Latitude.ToString() : string.Empty,
				Longitude = r.IsPhysical ? r.Location.Longitude.ToString() : string.Empty,

				ThumbnailId = r.ThumbnailId.HasValue ? r.ThumbnailId.ToString() : null,
				ImagesIds = r.Images != null ? r.Images.Select(r => r.ImageId.ToString()).ToList() : new List<string>(),
				Tags = r.TagDorehamies != null ? r.TagDorehamies.Select(r => r.Tag.Name).ToList() : new List<string>(),

				IsFavorited = userId.HasValue ? r.FavoritedUsers.Select(r => r.UserId).Contains(userId) : false,
				IsJoined = userId.HasValue ? r.JoinedUsers.Select(r => r.UserId).Contains(userId) : false,
			};
		}

		internal static Expression<Func<Dorehami, ObjectDorehamiSummery>> SummeryFromDorehami(long? userId)
		{
			return r => new ObjectDorehamiSummery
			{
				Id = r.Id.ToString(),
				Name = r.Name,
				CreatorId = r.CreatorId.ToString(),
				StartTime = r.StartTime,
				EndTime = r.EndTime,
				DateCreated = r.DateCreated,

				Summery = r.Summery,
				Category = r.Category,
				Size = r.Size,
				RemainingSize = r.JoinedUsers != null ? r.Size - r.JoinedUsers.Count :-1,

				IsPhysical = r.IsPhysical,
				Province = r.IsPhysical ? r.Province : string.Empty,
				Latitude = r.IsPhysical ? r.Location.Latitude.ToString() : string.Empty,
				Longitude = r.IsPhysical ? r.Location.Longitude.ToString() : string.Empty,

				ThumbnailId = r.ThumbnailId.HasValue ? r.ThumbnailId.ToString() : null,
				Tags = r.TagDorehamies != null ? r.TagDorehamies.Select(r => r.Tag.Name).ToList() : new List<string>(),

				IsFavorited = userId.HasValue ? r.FavoritedUsers.Select(r => r.UserId).Contains(userId) : false,
				IsJoined = userId.HasValue ? r.JoinedUsers.Select(r => r.UserId).Contains(userId) : false,
			};
		}

		internal static ObjectUserDetails DetailFromUser(User user)
		{
			return DetailFromUser().Compile()(user);
		}

		internal static Expression<Func<User, ObjectUserDetails>> DetailFromUser()
		{
			return r => new ObjectUserDetails
			{
				Email = r.Email,
				FirstName = r.FirstName,
				LastName = r.LastName,
				PhoneNumber = r.PhoneNumber,
				ProfilePictureId = r.ProfilePictureId.HasValue ? r.ProfilePictureId.ToString() : null,
			};
		}

		public static Image<Rgba32> ValidateImageFile(IFormFile file, out IImageFormat format)
		{
			byte[] img;
			using (var memoryStream = new MemoryStream())
			{
				file.CopyTo(memoryStream);
				memoryStream.Flush();
				img = memoryStream.ToArray();
			}

			format = SixLabors.ImageSharp.Image.DetectFormat(img);
			if (format == null)
			{
				throw new BaseException($"file {file.FileName} is not valid image file");
			}

			var image = SixLabors.ImageSharp.Image.Load(img);
			img = null;

			if (image == null)
			{
				throw new BaseException("bad image file //TODO");
			}

			//if (image.Height > documentTypeDefinition.MaxHeight)
			//{
			//	throw new MaximumSizeException("Height", documentTypeDefinition.MaxHeight);
			//}

			//if (image.Height < documentTypeDefinition.MinHeight)
			//{
			//	throw new MinimumSizeException("Height", documentTypeDefinition.MinHeight);
			//}

			//if (image.Width > documentTypeDefinition.MaxWidth)
			//{
			//	throw new MaximumSizeException("Width", documentTypeDefinition.MaxWidth);
			//}

			//if (image.Width < documentTypeDefinition.MinWidth)
			//{
			//	throw new MinimumSizeException("Width", documentTypeDefinition.MinWidth);
			//}

			return image;
		}

		public static async Task<sbyte> CalculateUserLevel(MainDbContext dbContext, long userId)
		{
			var joined = await dbContext.Users
				.Where(r => r.Id == userId)
				.Select(r => r.JoinedDorehamies.Count)
				.FirstOrDefaultAsync();

			sbyte level = 0;
			if (joined > 20)
			{
				level = 5;
			}
			else if (joined > 10)
			{
				level = 4;
			}
			else if (joined > 5)
			{
				level = 3;
			}
			else if (joined > 1)
			{
				level = 2;
			}
			else if (joined > 0)
			{
				level = 1;
			}
			else
			{
				level = 0;
			}

			return level;
		}

	}
}