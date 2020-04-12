namespace PatoghBackend.Exceptions.User
{
	using System.Net;

	using Exceptions.Common;

	public class DorehamiNotFavoritedException : BaseException
	{
		public DorehamiNotFavoritedException()
			: base(
				 Core.ErorrCodes.User.DorehamiNotFavorited,
				 Resources.Exceptions.User.DorehamiNotFavorited,
				 HttpStatusCode.BadRequest)
		{
		}
	}
}