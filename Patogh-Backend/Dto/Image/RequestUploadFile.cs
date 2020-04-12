namespace PatoghBackend.Dto.Image
{
	using Microsoft.AspNetCore.Http;
	using PatoghBackend.Core.Enums;

	public class RequestUploadImage
	{
		public IFormFile File { get; set; }

		public long UserId { get; set; }

		public ImageType ImageType { get; set; }
	}
}