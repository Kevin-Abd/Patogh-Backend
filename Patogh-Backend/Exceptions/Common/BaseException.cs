namespace PatoghBackend.Exceptions.Common
{
	using System;
	using System.Net;

	public class BaseException : Exception
	{
		public BaseException(string message)
		{
			ErrorCode = -1;
			HttpCode = HttpStatusCode.InternalServerError;
			if (!string.IsNullOrEmpty(message))
			{
				Message = message;
			}
		}

		public BaseException(
			int errorCode,
			string message = "",
			HttpStatusCode httpCode = HttpStatusCode.InternalServerError)
		{
			ErrorCode = errorCode;
			HttpCode = httpCode;
			if (!string.IsNullOrEmpty(message))
			{
				Message = message;
			}
		}

		public int ErrorCode { get; }

		public new string Message { get; set; }

		//public virtual object[] Properties => null;

		public HttpStatusCode HttpCode { get; set; }
	}
}