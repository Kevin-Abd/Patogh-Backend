namespace PatoghBackend.Entity.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Dorehami : Entity<long>
	{
		[StringLength(50)]
		public string Name { get; set; }

		public User Creator { get; set; }

		public long CreatorId { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public int Size { get; set; }

		[StringLength(100)]
		public string Summery { get; set; }

		[StringLength(1000)]
		public string Description { get; set; }

		[StringLength(50)]
		public string Category { get; set; }

		public bool IsPhysical { get; set; }

		public GeoLocation Location { get; set; }

		public long? LocationId { get; set; }

		[StringLength(200)]
		public string Address { get; set; }

		[StringLength(50)]
		public string Province { get; set; }

		public List<JoinUserDorehami> JoinedUsers { get; set; } = new List<JoinUserDorehami>();

		public List<FavUserDorehami> FavoritedUsers { get; set; } = new List<FavUserDorehami>();

		public List<ImageDorehami> Images { get; set; } = new List<ImageDorehami>();

		public Image Thumbnail { get; set; }

		public long? ThumbnailId { get; set; }

        public List<TagDorehami> TagDorehamies { get; set; } = new List<TagDorehami>();
	}
}