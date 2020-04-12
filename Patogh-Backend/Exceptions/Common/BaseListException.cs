namespace PatoghBackend.Exceptions.Common
{
	using System.Collections.Generic;

	public class BaseListException : BaseException
	{
		public BaseListException(IEnumerable<string> messages)
			: base(errorCode: -2)
		{
			Message = string.Join(",\n", messages);
		}
	}
}