namespace PatoghBackend.Dto.Dorehami
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class RequestCreateDorehami
	{
		[Required(ErrorMessage = "invalid request: name not found")]
		[MinLength(5)]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required(ErrorMessage = "invalid request: startTime not found")]
		public string StartTime { get; set; }

		[Required(ErrorMessage = "invalid request: endTime not found")]
		public string EndTime { get; set; }

		[MinLength(5)]
		[MaxLength(100)]
		[Required(ErrorMessage = "invalid request: summery not found")]
		public string Summery { get; set; }

		[Required(ErrorMessage = "invalid request: category not found")]
		public string Category { get; set; }

		[Required(ErrorMessage = "invalid request: size not found")]
		public int Size { get; set; }

		[Required(ErrorMessage = "invalid request: isPhysical not found")]
		public bool IsPhysical { get; set; }

		public string Latitude { get; set; }

		public string Longitude { get; set; }

		public string Province { get; set; }

		[Required(ErrorMessage = "invalid request: thumbnailId not found")]
		public string ThumbnailId { get; set; }

		[MinLength(5)]
		[MaxLength(100)]
		public string Address { get; set; }

		[MinLength(5)]
		[MaxLength(500)]
		[Required(ErrorMessage = "invalid request: description not found")]
		public string Description { get; set; }

		[Required(ErrorMessage = "invalid request: imagesIds not found")]
		public List<string> ImagesIds { get; set; } = new List<string>();

		[Required(ErrorMessage = "invalid request: tags not found")]
		public List<string> Tags { get; set; } = new List<string>();
	}
}