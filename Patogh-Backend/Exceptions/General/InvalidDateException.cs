namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class InvalidDateException : BaseException
	{
		public InvalidDateException(string propertyName, string value)
			: base(
				 Core.ErorrCodes.General.InvalidDate,
				 string.Empty,
				 HttpStatusCode.BadRequest)
		{
			Message = string.Format(
				Resources.Exceptions.General.InvalidDate,
				propertyName,
				value);
		}
	}
}