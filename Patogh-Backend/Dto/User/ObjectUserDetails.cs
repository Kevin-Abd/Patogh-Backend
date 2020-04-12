namespace PatoghBackend.Dto.User
{
	public class ObjectUserDetails
	{
		public string PhoneNumber { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string ProfilePictureId { get; set; }

		public sbyte UserLevel { get; set; } = -1;
	}
}