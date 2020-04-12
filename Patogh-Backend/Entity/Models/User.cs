namespace PatoghBackend.Entity.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Collections.Generic;

	public class User : Entity<long>
	{
		[StringLength(12)]
		public string PhoneNumber { get; set; }

		[StringLength(20)]
		public string FirstName { get; set; }

		[StringLength(30)]
		public string LastName { get; set; }

		[StringLength(30)]
		public string Email { get; set; }

		[StringLength(10)]
		public string LoginTokenValue { get; set; }

		public DateTime LoginTokenExpirationTime { get; set; }

		public string SessionToken { get; set; }

		public Image ProfilePicture { get; set; }

		public long? ProfilePictureId { get; set; }

		public List<JoinUserDorehami> JoinedDorehamies { get; set; } = new List<JoinUserDorehami>();

		public List<FavUserDorehami> FavoriteDorehamies { get; set; } = new List<FavUserDorehami>();
	}
}