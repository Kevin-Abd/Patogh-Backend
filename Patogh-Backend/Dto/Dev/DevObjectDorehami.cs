namespace PatoghBackend.Dto.Dorehami
{
	using System;

	public class DevObjectDorehami
	{
		public string Name { get; set; }

		public string CreatorPhone { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public int Size { get; set; }

		public string Description { get; set; }

		public string Summery { get; set; }

		public bool IsPhysical { get; set; }

		public string Latitude { get; set; }

		public string Longitude { get; set; }

		public string Address { get; set; }

		public string Province { get; set; }
	}
}