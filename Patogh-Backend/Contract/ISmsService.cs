namespace PatoghBackend.Contract
{
	using System.Threading.Tasks;

	using PatoghBackend.Dto.User;

	public interface ISmsService : IService
	{
		Task<string> SendSms(PhoneNumberWrapper phoneNumber, string message);
	}
}