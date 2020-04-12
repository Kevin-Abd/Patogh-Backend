namespace PatoghBackend.Dto.Common
{
	public class ApiResponse
	{
		public ApiResponse(int code, string messege)
		{
			Code = code;
			Message = messege ?? string.Empty;
		}

		public int Code { get; set; }

		public string Message { get; set; }
	}
}