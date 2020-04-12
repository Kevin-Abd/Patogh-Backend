namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class InvalidStringLengthException : BaseException
	{
		public InvalidStringLengthException(string propertyName, int len, int minLength, int maxLength)
			: base(
				 Core.ErorrCodes.General.InvalidStringLenght,
				 string.Empty,
				 HttpStatusCode.BadRequest)
		{
			Message = string.Format(
				Resources.Exceptions.General.InvalidStringLength,
				propertyName,
				len,
				minLength,
				maxLength);
		}
	}
}