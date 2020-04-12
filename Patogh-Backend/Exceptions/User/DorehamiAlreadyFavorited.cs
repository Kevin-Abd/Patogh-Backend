namespace PatoghBackend.Exceptions.User
{
	using System.Net;

	using Exceptions.Common;

	public class DorehamiAlreadyFavoritedException : BaseException
	{
		public DorehamiAlreadyFavoritedException()
			: base(
				 Core.ErorrCodes.User.DorehamiAlreadyFavorited,
				 Resources.Exceptions.User.DorehamiAlreadyFavorited,
				 HttpStatusCode.BadRequest)
		{
		}
	}
}