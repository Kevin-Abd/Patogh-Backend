namespace PatoghBackend.Validation
{
	using PatoghBackend.Core;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;
	using PatoghBackend.Exceptions.General;

	public static class DorehamiValidation
	{
		public static void ValidateCreateDorehami(RequestCreateDorehami request)
		{
			General.CheckIsLong(request.ThumbnailId);
			foreach (var id in request.ImagesIds)
			{
				General.CheckIsLong(id);
			}
			if (request.IsPhysical == true)
			{
				if (string.IsNullOrEmpty(request.Latitude) || !decimal.TryParse(request.Latitude, out var _))
				{
					throw new InvalidValueException(nameof(request.Latitude), request.Latitude);
				}
				if (string.IsNullOrEmpty(request.Longitude) || !decimal.TryParse(request.Longitude, out var _))
				{
					throw new InvalidValueException(nameof(request.Longitude), request.Longitude);
				}
				if (string.IsNullOrEmpty(request.Address))
				{
					throw new InvalidValueException(nameof(request.Address), request.Address);
				}
				if (string.IsNullOrEmpty(request.Province))
				{
					throw new InvalidValueException(nameof(request.Province), request.Province);
				}
			}

			var startTime = Core.PersianCalendarExtended.ToGregorianDateTime(request.StartTime);
			var endTime = Core.PersianCalendarExtended.ToGregorianDateTime(request.EndTime);
			if (startTime == null)
			{
				throw new InvalidDateException(nameof(request.StartTime), request.StartTime);
			}
			if (endTime == null)
			{
				throw new InvalidDateException(nameof(request.EndTime), request.EndTime);
			}
			if (startTime > endTime)
			{
				throw new InvalidValueException(nameof(request.StartTime), request.StartTime);
			}
		}

		public static void ValidateAndNormalizeAuthentication(RequestUserLogin request)
		{
			if (!General.IsDigitsOnly(request.LoginToken))
			{
				throw new InvalidValueException(nameof(RequestUserLogin.LoginToken), request.LoginToken);
			}

			General.ValidateStringLength(
				nameof(RequestUserLogin.LoginToken),
				request.LoginToken,
				Settings.Services.LoginTokenLength,
				Settings.Services.LoginTokenLength,
				false);

			request.PhoneNumber = General.ConvertPhoneNoTo12Char(
				nameof(request.PhoneNumber),
				request.PhoneNumber);
		}

		public static void ValidateAndNormalizeUserDetails(ObjectUserDetails userDetails)
		{
			userDetails.PhoneNumber = General.ConvertPhoneNoTo12Char(
				nameof(userDetails.PhoneNumber),
				userDetails.PhoneNumber);

			General.ValidateEmail(nameof(ObjectUserDetails.Email), userDetails.Email);

			General.ValidateStringLength(
				nameof(userDetails.FirstName),
				userDetails.FirstName,
				Settings.Validations.MinFirstNameLenght,
				Settings.Validations.MaxNameLenght,
				false);

			General.ValidateStringLength(
				nameof(userDetails.LastName),
				userDetails.LastName,
				Settings.Validations.MinLastNameLenght,
				Settings.Validations.MaxNameLenght,
				false);
		}
	}
}