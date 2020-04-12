namespace PatoghBackend.Entity.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Tag : Entity<long>
	{
		[StringLength(50)]
		public string Name { get; set; }

		public List<TagDorehami> TagDorehamies { get; set; } = new List<TagDorehami>();
	}
}