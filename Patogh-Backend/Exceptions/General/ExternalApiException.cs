namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class ExternalApiException : BaseException
	{
		public ExternalApiException(string serviceName, string value)
			: base(
				 Core.ErorrCodes.General.ExternalApiError,
				 string.Empty,
				 HttpStatusCode.InternalServerError)
		{
			Message = string.Format(
				Resources.Exceptions.General.ExternalApiError,
				serviceName,
				value);
		}
	}
}