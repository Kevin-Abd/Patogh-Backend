namespace PatoghBackend.Entity.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Collections.Generic;

	public class ImageDorehami : Entity<long>
	{
		public Image Image { get; set; }

		public long? ImageId { get; set; }

		public Dorehami Dorehami { get; set; }

		public long? DorehamiId { get; set; }
	}
}