namespace PatoghBackend.Dto.Dev
{
	using System.Collections.Generic;

	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;

	public class RequestSetThumbnail
	{
		public string DorehamiId { get; set; }

		public string ImageId { get; set; }
	}
}