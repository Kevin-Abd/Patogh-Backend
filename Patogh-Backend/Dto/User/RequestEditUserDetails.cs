namespace PatoghBackend.Dto.User
{
	using System.ComponentModel.DataAnnotations;

	public class RequestEditUserDetails
	{
		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public ObjectUserDetails UserDetails { get; set; }
	}
}