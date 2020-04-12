namespace PatoghBackend.Entity.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Collections.Generic;
	using PatoghBackend.Core.Enums;

	public class Image : Entity<long>
	{
		public User User { get; set; }

		public long UserId { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public ImageType Type { get; set; }
	}
}