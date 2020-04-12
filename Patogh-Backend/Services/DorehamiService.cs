namespace PatoghBackend.Services
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading.Tasks;

	using Core;

	using PatoghBackend.Dto.Dorehami;
	using Microsoft.EntityFrameworkCore;
	using PatoghBackend.Contract;
	using PatoghBackend.Data;
	using Services.Common;
	using System.Security.Claims;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Exceptions.General;
	using PatoghBackend.Entity.Models;
	using PatoghBackend.Core.Enums;

	public class DorehamiService : Service, IDorehamiService
	{
		private readonly MainDbContext dbContext;

		private readonly IUserService userService;

		private readonly ITagService tagService;

		private readonly IImageService imageService;

		private readonly IDeleteService deleteService;

		public DorehamiService(
			MainDbContext dbContext,
			IUserService userService,
			ITagService tagService,
			IImageService imageService,
			IDeleteService deleteService)
		{
			this.dbContext = dbContext;
			this.userService = userService;
			this.tagService = tagService;
			this.imageService = imageService;
			this.deleteService = deleteService;
		}

		public async Task<ObjectDorehamiDetails> Create(RequestCreateDorehami request, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			if (user == null)
			{
				throw new RecordNotFoundException(nameof(User));
			}

			var startTime = Core.PersianCalendarExtended.ToGregorianDateTime(request.StartTime);
			var endTime = Core.PersianCalendarExtended.ToGregorianDateTime(request.EndTime);

			if (!startTime.HasValue)
			{
				throw new InvalidDateException(nameof(request.StartTime), request.StartTime);
			}

			if (!endTime.HasValue)
			{
				throw new InvalidDateException(nameof(request.EndTime), request.EndTime);
			}

			var thumb = await dbContext.Images.AnyAsync(r => r.Id == long.Parse(request.ThumbnailId));
			if (!thumb)
			{
				throw new RecordNotFoundException(nameof(Image));
			}

			var imgIds = request.ImagesIds.Select(r => long.Parse(r));
			var ii = await dbContext.Images
				.Where(r => r.Type == ImageType.DorehamiAlbum && imgIds.Contains(r.Id))
				.CountAsync();
			if (ii != imgIds.Count())
			{
				throw new RecordNotFoundException(nameof(Image));
			}

			var dorehami = new Dorehami
			{
				Name = request.Name,
				DateCreated = DateTime.Now,
				Description = request.Description,
				Summery = request.Summery,
				CreatorId = user.Id,
				Size = request.Size,
				StartTime = startTime.Value,
				EndTime = endTime.Value,
				IsPhysical = request.IsPhysical,
				Address = request.IsPhysical ? request.Address : string.Empty,
				Province = request.IsPhysical ? request.Province : string.Empty,
				Category = request.Category,
				ThumbnailId = long.Parse(request.ThumbnailId),
			};
			dbContext.Dorehamies.Add(dorehami);
			await dbContext.SaveChangesAsync();

			//location
			GeoLocation location = null;
			if (request.IsPhysical)
			{
				location = new GeoLocation
				{
					Dorehami = dorehami,
					DorehamiId = dorehami.Id,
					Latitude = decimal.Parse(request.Latitude),
					Longitude = decimal.Parse(request.Longitude),
				};
				dorehami.Location = location;

				dbContext.GeoLocations.Add(location);
				await dbContext.SaveChangesAsync();

				dorehami.LocationId = location.Id;
			}

			// images
			var imageDorehamies = new List<ImageDorehami>();
			foreach (var imgId in imgIds)
			{
				var imageDor = new ImageDorehami
				{
					DorehamiId = dorehami.Id,
					ImageId = imgId,
				};
				dbContext.Add(imageDor);
			}
			await dbContext.SaveChangesAsync();

			//tags
			foreach (var tagName in request.Tags)
			{
				var tagDorehami = await tagService
					.AddTagToDorehami(new Dto.Tag.AddTagToDorehami
					{
						DorehamiId = dorehami.Id,
						TagName = tagName,
					});
			}

			return await dbContext.Dorehamies
				.Where(r => r.Id == dorehami.Id)
				.Select(Helper.DetailFromDorehami(user.Id))
				.FirstOrDefaultAsync();
		}

		public async Task<IdStringWrapper> UploadImage(FormFileWrapper request, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			var imageServiceReq = new Dto.Image.RequestUploadImage
			{
				File = request.File,
				UserId = user.Id,
				ImageType = Core.Enums.ImageType.DorehamiAlbum,
			};

			var res = await imageService.UploadImage(imageServiceReq);

			await dbContext.SaveChangesAsync();
			return res;
		}

		public async Task<ObjectDorehamiDetails> GetDetail(IdStringWrapper wrapper, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			var id = long.Parse(wrapper.IdString);

			var q0 = await dbContext.Dorehamies
				.AsNoTracking()
				.Where(r => r.Id == id)
				.Select(Helper.DetailFromDorehami(user.Id))
				.FirstOrDefaultAsync();


			return q0;
		}


		public async Task<List<ObjectDorehamiSummery>> GetSummery(ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			var q0 = dbContext.Dorehamies
				.AsNoTracking()
				.Select(Helper.SummeryFromDorehami(user.Id))
				.OrderByDescending(r => r.DateCreated);

			return await q0.ToListAsync();
		}

		public async Task<List<ObjectDorehamiSummery>> Search(RequestSearchDorehami request, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			var q0 = dbContext.Dorehamies
				.AsNoTracking();

			var q1 = q0;
			if (!string.IsNullOrEmpty(request.Name))
			{
				q1 = q1.Where(r => r.Name.Contains(request.Name));
			}

			if (!string.IsNullOrEmpty(request.Province))
			{
				q1 = q1.Where(r => r.Province == request.Province);
			}

			if (!string.IsNullOrEmpty(request.Category))
			{
				q1 = q1.Where(r => r.Category == request.Category);
			}

			if (request.Tags.Count != 0)
			{
				var tagsList = await tagService.GetTags(request.Tags);
				var tagIds = tagsList.Select(r => r.Id);
				q1 = q1.Where(r => r.TagDorehamies
								.Any(t => tagIds.Contains(t.TagId.Value)));
				//q1 = q1.Where(dor => tagIds.All(
				//				ti => dor.TagDorehamies
				//					.Select(td => td.TagId)
				//					.Contains(ti)
				//				));
			}

			var qf = q1
				.Select(Helper.SummeryFromDorehami(user.Id))
				.OrderByDescending(r => r.DateCreated);

			return await qf.ToListAsync();
		}


		public async Task DeleteDorehami(IdStringWrapper wrapper, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			var dorId = long.Parse(wrapper.IdString);
			var dor = await dbContext.Dorehamies
				.Where(r => r.Id == dorId)
				.FirstOrDefaultAsync();
			if ( dor.CreatorId != user.Id )
			{
				//todo new exception
				throw new InvalidRequestException("user does not own the dorehami");
			}
			else
			{
				 await deleteService.Dorehami(dor.Id);
			}
		}

		private bool disposed = false; // To detect redundant calls

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					dbContext.Dispose();
					userService.Dispose();
					// TODO: dispose managed state (managed objects
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposed = true;
				base.Dispose(disposing);
			}
		}
	}
}