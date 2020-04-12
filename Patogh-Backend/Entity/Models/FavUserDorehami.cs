namespace PatoghBackend.Entity.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Collections.Generic;

	public class FavUserDorehami : Entity<long>
	{
		public User User { get; set; }

		public long? UserId { get; set; }

		public Dorehami Dorehami { get; set; }

		public long? DorehamiId { get; set; }
	}
}