namespace PatoghBackend.Dto.Common
{
	public class ApiResponseT<T>
	{
		public ApiResponseT()
		{
		}

		public ApiResponseT(int code, string messege, T returnValue)
		{
			Code = code;
			Message = messege ?? string.Empty;
			ReturnValue = returnValue;
		}

		public int Code { get; set; }

		public string Message { get; set; }

		public T ReturnValue { get; set; }
	}
}