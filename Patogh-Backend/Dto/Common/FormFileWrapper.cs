namespace PatoghBackend.Dto.Common
{
	using System.ComponentModel.DataAnnotations;
	using Microsoft.AspNetCore.Http;

	public class FormFileWrapper
	{
		public IFormFile File { get; set; }
	}
}