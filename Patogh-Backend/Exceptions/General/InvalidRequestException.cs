namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class InvalidRequestException : BaseException
	{
		public InvalidRequestException(string message)
			: base(
				 Core.ErorrCodes.General.InvalidRequest,
				 message,
				 HttpStatusCode.BadRequest)
		{
		}

		public InvalidRequestException() : this(
			Resources.Exceptions.General.InvalidRequest)
		{ }
	}
}