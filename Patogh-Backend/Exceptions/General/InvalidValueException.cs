namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class InvalidValueException : BaseException
	{
		public InvalidValueException(string propertyName, string value)
			: base(
				 Core.ErorrCodes.General.InvalidValue,
				 string.Empty,
				 HttpStatusCode.BadRequest)
		{
			Message = string.Format(
					Resources.Exceptions.General.InvalidValue,
					propertyName,
					value);
		}
	}
}