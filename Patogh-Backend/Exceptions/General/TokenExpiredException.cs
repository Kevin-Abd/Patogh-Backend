namespace PatoghBackend.Exceptions.General
{
	using System.Net;

	using Exceptions.Common;

	public class TokenExpiredException : BaseException
	{
		public TokenExpiredException()
			: base(
				 Core.ErorrCodes.General.TokenExpired,
				 Resources.Exceptions.General.TokenExpired,
				 HttpStatusCode.NotFound)
		{
		}
	}
}