namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class FormFileNullException : BaseException
	{
		public FormFileNullException(string propertyName)
			: base(
				 Core.ErorrCodes.General.FormFileNull,
				 string.Format(Resources.Exceptions.General.FormFileNull, propertyName),
				 HttpStatusCode.BadRequest)
		{
		}
	}
}