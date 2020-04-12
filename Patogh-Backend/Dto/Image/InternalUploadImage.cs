namespace PatoghBackend.Dto.Image
{
	using System.IO;
	using Microsoft.AspNetCore.Http;
	using PatoghBackend.Core.Enums;

	public class InternalUploadImage
	{
		public Stream Stream { get; set; }

		public long UserId { get; set; }

		public ImageType ImageType { get; set; }

		public string ImageName { get; set; }
	}
}