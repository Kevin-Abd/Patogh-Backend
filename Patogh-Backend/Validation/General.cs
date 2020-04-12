namespace PatoghBackend.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using PatoghBackend.Core;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Exceptions.General;

	public static class General
	{
		public static void ValiadateFormFileWrapper(FormFileWrapper fileWrapper, bool optional = false)
		{
			if (fileWrapper.File == null)
			{
				if (optional)
				{
					return;
				}
				throw new FormFileNullException(nameof(fileWrapper.File));
			}

		}

		public static void CheckIsLong(string input, bool optional = false)
		{
			if (optional && string.IsNullOrEmpty(input))
			{
				return;
			}

			if (!long.TryParse(input, out long _))
			{
				throw new InvalidRequestException();
			}
		}

		public static void ValidateStringLength(string propertyName, string value, int min, int max, bool allowNull)
		{
			if (string.IsNullOrEmpty(value))
			{
				if (allowNull)
				{
					return;
				}

				throw new InvalidValueException(propertyName, value);
			}

			if (value.Length < min || value.Length > max)
			{
				throw new InvalidStringLengthException(propertyName, value.Length, min, max);
			}
		}

		public static void ValidateList<T>(IList<T> list)
		{
			if (list == null || list.Count == 0)
			{
				throw new InvalidRequestException();
			}
		}

		internal static bool IsDigitsOnly(string str)
		{
			foreach (var c in str)
			{
				if (c < '0' || c > '9')
				{
					return false;
				}
			}

			return true;
		}

		internal static void ValidateEmail(string propertyName, string value, bool allowNull = false)
		{
			if (value == null)
			{
				if (allowNull)
				{
					return;
				}

				throw new InvalidValueException(propertyName, value);
			}

			if (value.IndexOf('@', StringComparison.Ordinal) == -1 || value.IndexOf('.', StringComparison.Ordinal) == -1)
			{
				throw new InvalidValueException(propertyName, value);
			}

			try
			{
				var _ = new System.Net.Mail.MailAddress(value).Address;
			}
			catch
			{
				throw new InvalidValueException(propertyName, value);
			}
		}

		internal static string ConvertEmailOrPhoneNo(string propertyName, string value, bool allowNull = false)
		{
			if (value == null)
			{
				if (allowNull)
				{
					return null;
				}

				throw new InvalidValueException(propertyName, value);
			}

			ValidateStringLength(propertyName, value, 4, 50, allowNull);

			if (value.IndexOf('@', StringComparison.OrdinalIgnoreCase) > -1)
			{
				ValidateEmail(propertyName, value, allowNull);
				return value;
			}

			return ConvertPhoneNoTo12Char(propertyName, value, allowNull);
		}

		public static string ConvertPhoneNoTo12Char(string propertyName, string value, bool allowNull = false)
		{
			if (string.IsNullOrEmpty(value))
			{
				if (allowNull)
				{
					return null;
				}

				throw new InvalidValueException(propertyName, value);
			}

			if (value.Length > 13 || value.Length < 10)
			{
				throw new InvalidValueException(propertyName, value);
			}

			if (value.Length == 13)
			{
				if (!value.StartsWith("+989", StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidValueException(propertyName, value);
				}

				value = value.Substring(1);
			}

			if (!IsDigitsOnly(value))
			{
				throw new InvalidValueException(propertyName, value);
			}

			if (value.Length == 12)
			{
				if (!value.StartsWith("989", StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidValueException(propertyName, value);
				}

				return value;
			}

			if (value.Length == 11)
			{
				if (!value.StartsWith("09", StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidValueException(propertyName, value);
				}

				return "98" + value.Substring(1);
			}

			if (!value.StartsWith("9", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidValueException(propertyName, value);
			}

			return "98" + value;
		}

		internal static string ConvertDatetimeTo16CharGregorian(string propertyName, string input, bool optional = false)
		{
			if (optional && string.IsNullOrEmpty(input))
			{
				return null;
			}

			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentNullException(propertyName);
			}

			var dateTimeParts = input.Split(' ');

			if (dateTimeParts.Length > 2)
			{
				throw new InvalidDateException(propertyName, input);
			}

			if (dateTimeParts.Length == 1)
			{
				return ConvertDateTo10CharGregorian(propertyName, input, optional);
			}

			var time = dateTimeParts[1];
			if (time.Length > 5 || time.Length < 3 || time.IndexOf(':', StringComparison.InvariantCultureIgnoreCase) == -1)
			{
				throw new InvalidDateException(propertyName, input);
			}

			var timeParts = time.Split(':');

			if (timeParts.Length != 2)
			{
				throw new InvalidDateException(propertyName, input);
			}

			if (timeParts[0].Length == 1)
			{
				timeParts[0] = "0" + timeParts[0];
			}

			if (timeParts[1].Length == 1)
			{
				timeParts[1] = "0" + timeParts[1];
			}

			return $"{ConvertDateTo10CharGregorian(propertyName, dateTimeParts[0], optional)} {timeParts[0]}:{timeParts[1]}";
		}

		internal static string ConvertDateTo10CharGregorian(string propertyName, string input, bool optional = false)
		{
			if (optional && string.IsNullOrEmpty(input))
			{
				return null;
			}

			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentNullException(propertyName);
			}

			if (input.Length < 8 || input.Length > 10)
			{
				throw new InvalidDateException(propertyName, input);
			}

			foreach (var c in input)
			{
				if (c == '-' || c == '/' || c == '\\' || c == '_')
				{
					continue;
				}

				if (c < '0' || c > '9')
				{
					throw new InvalidDateException(propertyName, input);
				}
			}

			var parts = input.Split('-', '/', '\\', '_');

			if (parts.Length != 3 || parts[0].Length != 4)
			{
				throw new InvalidDateException(propertyName, input);
			}

			if (!int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out var y))
			{
				throw new InvalidDateException(propertyName, input);
			}

			if (!int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var m))
			{
				throw new InvalidDateException(propertyName, input);
			}

			if (!int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out var d))
			{
				throw new InvalidDateException(propertyName, input);
			}

			var result = $"{y}/{m:00}/{d:00}";
			if (y > 1900)
			{
				if (!DateTime.TryParseExact(result, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out _))
				{
					throw new InvalidValueException(propertyName, input);
				}
			}
			else
			{
				var date = PersianCalendarExtended.ToGregorian(result);
				if (!date.HasValue)
				{
					throw new InvalidValueException(propertyName, input);
				}

				result = date.Value.ToString("yyyy/MM/dd");
			}

			return result;
		}
	}
}

//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Validation
//{
//	public static class General
//	{
//		private static readonly TimeSpan MinAge = TimeSpan.FromDays(1 * 365);
//		private static readonly TimeSpan MaxAge = TimeSpan.FromDays(150 * 365);

//
//		public static void ValidateSearchByKeyword(string value)
//		{
//			if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
//			{
//				//throw new InvalidRequestException();
//			}
//		}

//		public static void ValidatePostalCode(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			if (value.Length != 10 || !IsDigitsOnly(value))
//			{
//				throw new InvalidValueException(propertyName, value);
//			}
//		}

//		public static void ValidateDouble(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			if (!double.TryParse(value, out _))
//			{
//				throw new InvalidValueException(propertyName, value);
//			}
//		}

//		internal static void ValidateNationalCode(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			if (!IsValidNationalCode(value))
//			{
//				throw new InvalidValueException(propertyName, value);
//			}
//		}

//		internal static void ValidateTel(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			if (value.Length < 1)
//			{
//				throw new InvalidValueException(propertyName, value);
//			}

//			foreach (var c in value)
//			{
//				if ((c < '0' || c > '9') && c != ' ' && c != '-' && c != '+' && c != '(' && c != ')')
//				{
//					throw new InvalidValueException(propertyName, value);
//				}
//			}
//		}

//		internal static void ValidateVerificationCode(string propertyName, int? value, bool allowNull = false)
//		{
//			if (!value.HasValue)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, "null");
//			}

//			if (value > 999999 || value < 100000)
//			{
//				throw new InvalidValueException(propertyName, value.Value.ToString());
//			}
//		}

//		internal static void ValidateUsername(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			ValidateStringLength(propertyName, value, 4, 40, allowNull);
//			if (value.IndexOf('@', StringComparison.InvariantCulture) >= 0)
//			{
//				throw new BaseException("username must not contain @");
//			}

//			if (!char.IsLetter(value[0]))
//			{
//				throw new BaseException("username must start with letter character");
//			}

//			if (Kookaat.Core.Setting.General.ReservedUsernames.Contains(value.ToUpperInvariant()))
//			{
//				throw new ReservedValueException(value);
//			}
//		}

//		internal static void ValidateProfileImage(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			if (value.Length < 1024)
//			{
//				throw new FileTooSmallException("profile image", 1);
//			}

//			if (value.Length > 1024 * Kookaat.Core.Setting.User.ProfileImageMaxSizeKiloBytes)
//			{
//				throw new FileTooBigException("profile image", Kookaat.Core.Setting.User.ProfileImageMaxSizeKiloBytes);
//			}
//		}

//		internal static void ValidatePersonalIdNumber(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			ValidateStringLength(propertyName, value, 1, 10, allowNull);

//			if (!IsDigitsOnly(value))
//			{
//				throw new InvalidValueException(propertyName, value);
//			}
//		}

//		internal static void ValidateEconomicCode(string propertyName, string value, bool allowNull = false)
//		{
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(propertyName, value);
//			}

//			ValidateStringLength(propertyName, value, 10, 16, allowNull);

//			if (!IsDigitsOnly(value))
//			{
//				throw new InvalidValueException(propertyName, value);
//			}
//		}

//		internal static void ValidateFile(IFormFile file, FileType fileType)
//		{
//			ValidateFile(file, new[] { fileType });
//		}

//		internal static void ValidateFile(string filename, FileType fileType)
//		{
//			ValidateFile(filename, new[] { fileType });
//		}

//		internal static void ValidateFile(IFormFile file, FileType[] fileTypes)
//		{
//			if (file.Length == 0)
//			{
//				throw new EmptyFileException(file.FileName);
//			}

//			ValidateFile(file.FileName, fileTypes);
//		}

//		internal static void ValidateFiles(List<IFormFile> files, List<FileType> fileTypes)
//		{
//			foreach (var file in files)
//			{
//				if (!fileTypes.Any(r => file.FileName.EndsWith(r.ToString(), StringComparison.InvariantCultureIgnoreCase)))
//				{
//					throw new InvalidFileTypeException(file.FileName);
//				}
//			}
//		}

//		internal static void ValidateDecimalString(string input, string name, bool allowNull, byte maxValidScale, bool allowNegative = true, bool allowPositive = true, bool allowZero = true, decimal? min = null, decimal? max = null)
//		{
//			if (input == null)
//			{
//				if (allowNull)
//				{
//					return;
//				}

//				throw new InvalidValueException(name, input);
//			}

//			if (!decimal.TryParse(input, out var value))
//			{
//				throw new BaseException($"{input} is not decimal");
//			}

//			ValidateDecimal(value, name, maxValidScale, allowNegative, allowPositive, allowZero, min, max);
//		}

//		internal static void ValidateDecimal(decimal value, string name, byte maxValidScale, bool allowNegative = true, bool allowPositive = true, bool allowZero = true, decimal? min = null, decimal? max = null)
//		{
//			if (!allowNegative && value < 0)
//			{
//				throw new BaseException($"{name} cannot be negative");
//			}

//			if (!allowPositive && value > 0)
//			{
//				throw new BaseException($"{name} cannot be positive");
//			}

//			if (!allowZero && value == 0)
//			{
//				throw new BaseException($"{name} cannot be zero");
//			}

//			if (!allowZero && value == 0)
//			{
//				throw new BaseException($"{name} cannot be zero");
//			}

//			if (min.HasValue && value < min.Value)
//			{
//				throw new BaseException($"{name} cannot be lower than {min.Value}");
//			}

//			if (max.HasValue && value > max.Value)
//			{
//				throw new BaseException($"{name} cannot be higher than {max.Value}");
//			}

//			if ((byte)((decimal.GetBits(value)[3] >> 16) & 0x7F) > maxValidScale)
//			{
//				throw new BaseException($"{name} max scale should be {maxValidScale}");
//			}
//		}

//		internal static long ConvertToLong(string input)
//		{
//			if (!long.TryParse(input, out long output))
//			{
//				throw new InvalidRequestException();
//			}

//			return output;
//		}

//		internal static string ConvertBirthDateTo10CharGregorian(string propertyName, string value, bool allowNull = false)
//		{
//			value = ConvertDateTo10CharGregorian(propertyName, value, allowNull);
//			if (value == null)
//			{
//				if (allowNull)
//				{
//					return null;
//				}

//				throw new InvalidValueException(propertyName, "null");
//			}

//			var format = value.IndexOf('/', StringComparison.Ordinal) > 0 ? "yyyy/MM/dd" : "yyyy-MM-dd";
//			var date = DateTime.ParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

//			if (DateTime.Now - date < MinAge)
//			{
//				throw new BaseException($"{propertyName} is too young");
//			}

//			if (DateTime.Now - date > MaxAge)
//			{
//				throw new BaseException($"{propertyName} is too old");
//			}

//			return value;
//		}

//		internal static bool IsDigitsOnly(string str)
//		{
//			foreach (var c in str)
//			{
//				if (c < '0' || c > '9')
//				{
//					return false;
//				}
//			}

//			return true;
//		}

//		private static bool IsValidNationalCode(string nationalCode)
//		{
//			if (nationalCode == null || nationalCode.Length != 10)
//			{
//				return false;
//			}

//			if (!IsDigitsOnly(nationalCode))
//			{
//				return false;
//			}

//			if (nationalCode[0] == nationalCode[1] && nationalCode[0] == nationalCode[2] && nationalCode[0] == nationalCode[3]
//				&& nationalCode[0] == nationalCode[4] && nationalCode[0] == nationalCode[5] && nationalCode[0] == nationalCode[6]
//				&& nationalCode[0] == nationalCode[7] && nationalCode[0] == nationalCode[8] && nationalCode[0] == nationalCode[9])
//			{
//				return false;
//			}

//			var b = 0;
//			for (int ii = 0; ii < 9; ii++)
//			{
//				b += int.Parse(nationalCode[ii].ToString()) * (10 - ii);
//			}

//			var a = Convert.ToInt32(nationalCode[9].ToString());

//			var c = b % 11;

//			return ((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a));
//		}
//	}
//}