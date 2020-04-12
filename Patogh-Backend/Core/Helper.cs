namespace PatoghBackend.Core
{
	using System;
	using System.Globalization;

	public static class PersianCalendarExtended
	{
		public const string LtrOverride = "‭";

		private static readonly DateTime OldestValidDateTime = new DateTime(1900, 1, 1);

		private static readonly DateTime NewestValidDateTime = new DateTime(2099, 1, 1);

		private static readonly PersianCalendar FarsiCalendar = new PersianCalendar();

		public static string TodayDateWithCustomSeparator => $"{FarsiCalendar.GetYear(DateTime.Now)}{{0}}{FarsiCalendar.GetMonth(DateTime.Now):00}{{0}}{FarsiCalendar.GetDayOfMonth(DateTime.Now):00}";

		public static string Today16Char
		{
			get
			{
				var now = DateTime.Now;
				return $"{FarsiCalendar.GetYear(now)}/{FarsiCalendar.GetMonth(now):00}/{FarsiCalendar.GetDayOfMonth(now):00} {now.Hour:00}:{now.Minute:00}";
			}
		}

		public static string Today16CharForFilename
		{
			get
			{
				var now = DateTime.Now;
				return $"{FarsiCalendar.GetYear(now)}-{FarsiCalendar.GetMonth(now):00}-{FarsiCalendar.GetDayOfMonth(now):00}_{now.Hour:00}-{now.Minute:00}";
			}
		}

		public static string Today12Char
		{
			get
			{
				var now = DateTime.Now;
				return $"{FarsiCalendar.GetYear(now)}/{FarsiCalendar.GetMonth(now):00}/{FarsiCalendar.GetDayOfMonth(now):00}";
			}
		}

		public static void ForceConstructor()
		{
		}

		public static DateTime? ToGregorian(string date)
		{
			try
			{

				if (string.IsNullOrEmpty(date))
				{
					return null;
				}

				var dateParts = date.Split('/', '-');
				if (dateParts.Length != 3)
				{
					if (dateParts.Length == 1 && date.Length == 8)
					{
						dateParts = new string[3];
						dateParts[0] = date.Substring(0, 4);
						dateParts[1] = date.Substring(4, 2);
						dateParts[2] = date.Substring(6, 2);
					}
					else
					{
						return null;
					}
				}

				var q = FarsiCalendar.ToDateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), 0, 0, 0, 0);
				return q < OldestValidDateTime || q > NewestValidDateTime ? (DateTime?)null : q;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// expected format "yy/MM/dd hh:mm"
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime? ToGregorianDateTime(string date)
		{
			try
			{
				if (string.IsNullOrEmpty(date))
				{
					return null;
				}
				var dateString = date.Substring(0, 8);
				var timeString = date.Substring(9, 5);

				var dateParts = dateString.Split('/', '-');
				var timeParts = timeString.Split(':');

				if (dateParts.Length != 3 || timeParts.Length != 2)
				{
					return null;
				}

				var y = int.Parse(dateParts[0]);
				y += y > 90 && y < 1300 ? 1300 : 1400;

				var q = FarsiCalendar.ToDateTime(
					year: y,
					month: int.Parse(dateParts[1]),
					day: int.Parse(dateParts[2]),
					hour: int.Parse(timeParts[0]),
					minute: int.Parse(timeParts[1]),
					0,
					0);
				return q < OldestValidDateTime || q > NewestValidDateTime ? (DateTime?)null : q;
			}
			catch
			{
				return null;
			}
		}

		public static TimeSpan? ToTimeSpan(string time)
		{
			if (string.IsNullOrEmpty(time))
			{
				return null;
			}

			var timeParts = time.Split('.');
			if (timeParts.Length < 4)
			{
				return null;
			}

			foreach (var timePart in timeParts)
			{
				if (!int.TryParse(timePart, out _))
				{
					return null;
				}
			}

			return new TimeSpan(0, int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]), int.Parse(timeParts[3]));
		}

		public static string ToPersianDate(DateTime? dateTime, string separator = "/") => dateTime == null ? string.Empty : $"{FarsiCalendar.GetYear(dateTime.Value)}{separator}{FarsiCalendar.GetMonth(dateTime.Value):00}{separator}{FarsiCalendar.GetDayOfMonth(dateTime.Value):00}";

		public static string ToPersianDateTime(DateTime dateTime, string separator = "/", bool enableLtrOverride = true) => $"{FarsiCalendar.GetYear(dateTime)}{separator}{FarsiCalendar.GetMonth(dateTime):00}{separator}{FarsiCalendar.GetDayOfMonth(dateTime):00}{(enableLtrOverride ? LtrOverride : string.Empty)} {dateTime.Hour:00}:{dateTime.Minute:00}";

		public static string ToPersianDateTimeWithMillisecondPrecision(DateTime dateTime) =>
			$"{FarsiCalendar.GetYear(dateTime)}/{FarsiCalendar.GetMonth(dateTime):00}/{FarsiCalendar.GetDayOfMonth(dateTime):00}{LtrOverride} {dateTime.Hour:00}:{dateTime.Minute:00}:{dateTime.Second:00}.{dateTime.Millisecond:000}";

		public static short GetPersianYear(DateTime dateTime) => (short)FarsiCalendar.GetYear(dateTime);

		public static byte GetPersianMonth(DateTime dateTime) => (byte)FarsiCalendar.GetMonth(dateTime);

		//public static DateSimple ToPersianDateSimple(in DateTime dateTime)
		//{
		//    var result = new DateSimple
		//               {
		//                   Year = (short)FarsiCalendar.GetYear(dateTime),
		//                   Month = (byte)FarsiCalendar.GetMonth(dateTime),
		//               };

		//    switch (FarsiCalendar.GetDayOfWeek(dateTime))
		//    {
		//        case DayOfWeek.Friday:
		//            result.DayOfWeek = 7;
		//            break;
		//        case DayOfWeek.Monday:
		//            result.DayOfWeek = 3;
		//            break;
		//        case DayOfWeek.Saturday:
		//            result.DayOfWeek = 1;
		//            break;
		//        case DayOfWeek.Sunday:
		//            result.DayOfWeek = 2;
		//            break;
		//        case DayOfWeek.Thursday:
		//            result.DayOfWeek = 6;
		//            break;
		//        case DayOfWeek.Tuesday:
		//            result.DayOfWeek = 4;
		//            break;
		//        case DayOfWeek.Wednesday:
		//            result.DayOfWeek = 5;
		//            break;
		//    }

		//    result.Week = (byte)((13 + FarsiCalendar.GetDayOfYear(dateTime) - result.DayOfWeek) / 7);

		//    return result;
		//}
	}
}