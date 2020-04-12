namespace PatoghBackend.Dto.Dorehami
{
	using System.Collections.Generic;

	public class ObjectDorehamiDetails : ObjectDorehamiSummery
	{
		public string Address { get; set; }

		public string Description { get; set; }

		public List<string> ImagesIds { get; set; } = new List<string>();
	}
}