namespace PatoghBackend.Dto.Dorehami
{
	using System;
	using Microsoft.AspNetCore.Http;

	public class DevAddPictureDorehami
	{
		public string DorehamiId { get; set; }

		public IFormFile File { get; set; }
	}
}