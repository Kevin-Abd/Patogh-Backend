namespace PatoghBackend.Core
{
	public static class Settings
	{
		public static class Validations
		{
			public const int MinFirstNameLenght = 2;

			public const int MinLastNameLenght = 3;

			public const int MaxNameLenght = 100;
		}

		public static class Services
		{
			public const string ParsGreenSms = "[REDACTED]";

			public const string ParsGreenSingnature = "[REDACTED]";

			public const int LoginTokenLength = 4;

			public const int FileFactor = 1024;

			public static class Images
			{
				public const string Directory = "/Images";

				//thumbnail
				public const int ProfileThumbnailWidth = 128;

				public const int ProfileThumbnailHeight = 128;

				public const int DorehamiThumbnailWidth = 512;

				public const int DorehamiThumbnailHeight = 256;

				//orginal
				public const int ProfileMaxWidth = 500;

				public const int ProfileMaxHeight = 500;

				public const int DorehamiMaxWidth = 1000;

				public const int DorehamiMaxHeight = 500;
			}
		}
	}
}