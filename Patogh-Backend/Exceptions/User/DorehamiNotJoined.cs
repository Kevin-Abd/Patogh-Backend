namespace PatoghBackend.Exceptions.User
{
	using System.Net;

	using Exceptions.Common;

	public class DorehamiNotJoinedException : BaseException
	{
		public DorehamiNotJoinedException()
			: base(
				 Core.ErorrCodes.User.DorehamiNotJoined,
				 Resources.Exceptions.User.DorehamiNotJoined,
				 HttpStatusCode.BadRequest)
		{
		}
	}
}