namespace PatoghBackend.Dto.Dorehami
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class RequestSearchDorehami
	{
		// search items
		// Title : string
		// Catagory : string
		// Provice : string
		// Tags : List<string>
		// StartTime : time span
		// EndTime : time span
		// Diration : time span
		// Remaining size : int span
		// Physical : bool


		[MaxLength(50)]
		public string Name { get; set; }

		[MaxLength(50)]
		public string Category { get; set; }

		[MaxLength(50)]
		public string Province { get; set; }

		public List<string> Tags { get; set; } = new List<string>();
	}
}