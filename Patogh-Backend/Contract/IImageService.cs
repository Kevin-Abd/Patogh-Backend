namespace PatoghBackend.Contract
{
	using System.IO;
	using System.Threading.Tasks;

	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Image;

	public interface IImageService : IService
	{
		Task<IdStringWrapper> UploadImage(RequestUploadImage request);

		Task<Stream> DownloadImage(IdStringWrapper wrapper);

		Task<Stream> DownloadThumbnail(IdStringWrapper wrapper);
	}
}