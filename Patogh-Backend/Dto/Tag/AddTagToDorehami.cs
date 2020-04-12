namespace PatoghBackend.Dto.Tag
{
	using System.IO;
	using Microsoft.AspNetCore.Http;
	using PatoghBackend.Core.Enums;

	public class AddTagToDorehami
	{
		public long DorehamiId { get; set; }

		public string TagName { get; set; }
	}
}