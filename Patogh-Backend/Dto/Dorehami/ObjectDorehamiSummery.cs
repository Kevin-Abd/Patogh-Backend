namespace PatoghBackend.Dto.Dorehami
{
	using System;
	using System.Collections.Generic;

	public class ObjectDorehamiSummery
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string CreatorId { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public string Summery { get; set; }

		public string Category { get; set; }

		public int Size { get; set; }

		public int RemainingSize { get; set; }

		public bool IsPhysical { get; set; }

		public string Latitude { get; set; }

		public string Longitude { get; set; }

		public string Province { get; set; }

		public string ThumbnailId { get; set; }

		public bool IsJoined { get; set; }

		public bool IsFavorited { get; set; }

		public List<string> Tags { get; set; } = new List<string>();
	}
}