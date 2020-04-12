namespace PatoghBackend.Dto.User
{
	using System.ComponentModel.DataAnnotations;

	public class PhoneNumberWrapper
	{
		[Required]
		public string PhoneNumber { get; set; }
	}
}