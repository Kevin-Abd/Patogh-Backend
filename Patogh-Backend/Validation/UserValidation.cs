namespace PatoghBackend.Validation
{
	using PatoghBackend.Core;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.User;
	using PatoghBackend.Exceptions.General;

	public static class UserValidation
	{
		public static void ValidateAndNormalizePhoneNumber(PhoneNumberWrapper phoneNumber)
		{
			phoneNumber.PhoneNumber = General.ConvertPhoneNoTo12Char(
				nameof(phoneNumber),
				phoneNumber.PhoneNumber);
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

		public static void ValidateUplloadProfilePictuer(FormFileWrapper fileWrapper)
		{
			if (fileWrapper.File == null)
			{
				throw new FormFileNullException(nameof(FormFileWrapper.File));
			}
		}
	}
}