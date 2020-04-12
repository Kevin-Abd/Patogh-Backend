namespace PatoghBackend.Exceptions.User
{
	using System.Net;

	using Exceptions.Common;

	public class DorehamiAlreadyJoinedException : BaseException
	{
		public DorehamiAlreadyJoinedException()
			: base(
				 Core.ErorrCodes.User.DorehamiAlreadyJoined,
				 Resources.Exceptions.User.DorehamiAlreadyJoined,
				 HttpStatusCode.BadRequest)
		{
		}
	}
}