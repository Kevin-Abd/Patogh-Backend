namespace PatoghBackend.Core
{
	public static class ErorrCodes
	{
		public static class General
		{
			public const int ExternalApiError = -101;
			public const int InvalidDate = -102;
			public const int InvalidRequest = -103;
			public const int InvalidStringLenght = -104;
			public const int InvalidValue = -105;
			public const int RecordNotFound = -106;
			public const int TokenExpired = -107;
			public const int AggregateException = -108;
			public const int InternalError = -109;
			public const int FormFileNull = -110;
		}

		public static class User
		{
			public const int NoLoginToken = -401;
			public const int DorehamiAlreadyFavorited = -402;
			public const int DorehamiAlreadyJoined = -403;
			public const int DorehamiNotFavorited = -404;
			public const int DorehamiNotJoined = -405;
		}
	}
}