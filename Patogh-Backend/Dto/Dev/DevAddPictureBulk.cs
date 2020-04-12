namespace PatoghBackend.Dto.Dorehami
{
	using System;
	using System.Collections.Generic;
	using Microsoft.AspNetCore.Http;

	public class DevAddPictureBulk
	{
		public string DorehamiId { get; set; }

		public List<IFormFile> Files { get; set; }
	}
}