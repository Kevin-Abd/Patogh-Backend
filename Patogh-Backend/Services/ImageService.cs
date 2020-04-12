namespace PatoghBackend.Services
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	using Exceptions.General;

	using Microsoft.EntityFrameworkCore;

	using PatoghBackend.Contract;
	using PatoghBackend.Core;
	using PatoghBackend.Core.Enums;
	using PatoghBackend.Data;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Image;
	using PatoghBackend.Entity.Models;

	using Services.Common;

	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.Processing;

	public class ImageService : Service, IImageService
	{
		private readonly MainDbContext dbContext;

		public ImageService(MainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		private bool disposed = false; // To detect redundant calls

		public async Task<IdStringWrapper> UploadImage(RequestUploadImage request)
		{
			var user = await dbContext.Users
				.Where(r => r.Id == request.UserId)
				.FirstOrDefaultAsync();
			if (user == null)
			{
				throw new RecordNotFoundException(nameof(User));
			}

			//var slImage = Helper.ValidateImageStream(upload.Stream, out var format);
			var slImage = Helper.ValidateImageFile(request.File, out var format);

			var fName = request.File.Name.Length > 40 ?
				request.File.Name.Substring(0, 40) :
				request.File.Name;

			var image = new Entity.Models.Image
			{
				Name = fName,
				UserId = user.Id,
				Height = slImage.Height,
				Width = slImage.Width,
				Type = request.ImageType,
			};

			dbContext.Add(image);

			await dbContext.SaveChangesAsync();

			var dir = $"{Directory.GetCurrentDirectory()}/" +
				$"{Settings.Services.Images.Directory}/" +
				$"{image.Id / Settings.Services.FileFactor}/";

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			//image
			var filePath = $"{dir}{image.Id}.png";
			var inputResize = InputResize(request.ImageType, slImage.Width, slImage.Height);
			slImage.Mutate(x => x.Resize(inputResize));
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				slImage.SaveAsPng(fileStream);
				await fileStream.FlushAsync();
			}

			//thumbnail
			filePath = $"{dir}{image.Id}.thumb.jpg";
			var thumnailResize = ThumbnailResize(request.ImageType, slImage.Width, slImage.Height);
			slImage.Mutate(x => x.Resize(thumnailResize));

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				slImage.SaveAsJpeg(fileStream);
				await fileStream.FlushAsync();
			}

			slImage.Dispose();
			return new IdStringWrapper { IdString = image.Id.ToString() };
		}

		public async Task<Stream> DownloadImage(IdStringWrapper wrapper)
		{
			var id = long.Parse(wrapper.IdString);
			var image = await dbContext.Images
				.Where(r => r.Id == id)
				.FirstOrDefaultAsync();
			if (image == null)
			{
				throw new RecordNotFoundException(nameof(Entity.Models.Image));
			}

			var dir = $"{Directory.GetCurrentDirectory()}/" +
				$"{Settings.Services.Images.Directory}/" +
				$"{image.Id / Settings.Services.FileFactor}/";

			var filePath = $"{dir}{image.Id}.png";

			var memStream = new MemoryStream();
			using (var file = new FileStream(filePath, FileMode.Open))
				await file.CopyToAsync(memStream);

			return memStream;
		}

		public async Task<Stream> DownloadThumbnail(IdStringWrapper wrapper)
		{
			var id = long.Parse(wrapper.IdString);
			var image = await dbContext.Images
				.Where(r => r.Id == id)
				.FirstOrDefaultAsync();
			if (image == null)
			{
				throw new RecordNotFoundException(nameof(Entity.Models.Image));
			}

			var dir = $"{Directory.GetCurrentDirectory()}/" +
				$"{Settings.Services.Images.Directory}/" +
				$"{image.Id / Settings.Services.FileFactor}/";

			var filePath = $"{dir}{image.Id}.thumb.jpg";

			var memStream = new MemoryStream();
			try
			{
				using var file = new FileStream(filePath, FileMode.Open);
				await file.CopyToAsync(memStream);
			}
			catch (FileNotFoundException e)
			{
				throw;
			}
			catch (IOException e)
			{
				throw;
			}
			return memStream;
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					dbContext.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposed = true;
				base.Dispose(disposing);
			}
		}

		private ResizeOptions InputResize(ImageType type, int width, int height)
		{
			var resize = new ResizeOptions();
			var size = new SixLabors.Primitives.Size();
			var min = 0;
			switch (type)
			{
				case ImageType.Profile:
					resize.Mode = ResizeMode.Crop;
					min = Math.Min(width, height);
					size.Width = Settings.Services.Images.ProfileMaxWidth;
					size.Height = Settings.Services.Images.ProfileMaxHeight;
					resize.Size = size;
					break;

				case ImageType.DorehamiAlbum:
					resize.Mode = ResizeMode.Crop;
					size.Height = Settings.Services.Images.DorehamiMaxHeight;
					size.Width = Settings.Services.Images.DorehamiMaxWidth;
					resize.Size = size;
					break;

				case ImageType.UnSet:
					throw new InvalidValueException(nameof(ImageType), type.ToString());
			}
			return resize;
		}

		private ResizeOptions ThumbnailResize(ImageType type, int width, int height)
		{
			var resize = new ResizeOptions();
			var size = new SixLabors.Primitives.Size();
			switch (type)
			{
				case ImageType.Profile:
					resize.Mode = ResizeMode.Crop;
					size.Width = Settings.Services.Images.ProfileThumbnailWidth;
					size.Height = Settings.Services.Images.ProfileThumbnailHeight;
					resize.Size = size;
					break;

				case ImageType.DorehamiAlbum:
					resize.Mode = ResizeMode.Crop;
					size.Width = Settings.Services.Images.DorehamiThumbnailWidth;
					size.Height = Settings.Services.Images.DorehamiThumbnailHeight;
					resize.Size = size;
					break;

				case ImageType.UnSet:
					throw new InvalidValueException(nameof(ImageType), type.ToString());
			}
			return resize;
		}
	}
}