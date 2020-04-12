namespace PatoghBackend.Exceptions.User
{
	using System.Net;

	using Exceptions.Common;

	public class NoLoginTokenException : BaseException
	{
		public NoLoginTokenException()
			: base(
				 Core.ErorrCodes.User.NoLoginToken,
				 Resources.Exceptions.User.NoLoginTokenAvailable,
				 HttpStatusCode.BadRequest)
		{
		}
	}
}