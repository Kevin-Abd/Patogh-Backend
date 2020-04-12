namespace PatoghBackend.Entity.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Collections.Generic;

	public class TagDorehami : Entity<long>
	{
		public Tag Tag { get; set; }

		public long? TagId { get; set; }

		public Dorehami Dorehami { get; set; }

		public long? DorehamiId { get; set; }
	}
}