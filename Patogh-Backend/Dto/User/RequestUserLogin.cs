namespace PatoghBackend.Dto.User
{
	using System.ComponentModel.DataAnnotations;

	public class RequestUserLogin
	{
		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string LoginToken { get; set; }
	}
}