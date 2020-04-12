namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class RecordNotFoundException : BaseException
	{
		public RecordNotFoundException(string recordName)
			: base(
				 Core.ErorrCodes.General.RecordNotFound,
				 string.Empty,
				 HttpStatusCode.NotFound)
		{
			Message = string.Format(
				Resources.Exceptions.General.RecordNotFound,
				recordName);
		}
	}
}