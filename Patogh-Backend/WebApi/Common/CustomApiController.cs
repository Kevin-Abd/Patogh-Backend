namespace PatoghBackend.WebApi.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;

	using Exceptions.Common;

	using Microsoft.AspNetCore.Mvc;

	using PatoghBackend.Dto.Common;

	public class CustomApiController : ControllerBase
	{
		private static readonly OkObjectResult Result = new OkObjectResult(new ApiResponse(0, string.Empty));
#pragma warning disable CA1031 // Do not catch general exception types

		protected async Task<IActionResult> ExecuteAsync(Func<Task> serviceDelegate, Action validatorAction = null)
		{
			try
			{
				validatorAction?.Invoke();
				await serviceDelegate.Invoke();
				return Result;
			}
			catch (Exception exception)
			{
				return HandleException(exception);
			}
		}

		protected IActionResult Execute(Action serviceDelegate, Action validatorAction = null)
		{
			try
			{
				validatorAction?.Invoke();
				serviceDelegate.Invoke();
				return Result;
			}
			catch (Exception exception)
			{
				return HandleException(exception);
			}
		}

		protected async Task<IActionResult> ExecuteAsync<T>(Func<Task<T>> serviceDelegate, Action validatorAction = null)
		{
			try
			{
				validatorAction?.Invoke();
				return new OkObjectResult(new ApiResponseT<T> { Code = 0, Message = string.Empty, ReturnValue = await serviceDelegate.Invoke() });
			}
			catch (Exception exception)
			{
				return HandleException(exception);
			}
		}

		protected IActionResult Execute<T>(Func<T> serviceDelegate, Action validatorAction = null)
		{
			try
			{
				validatorAction?.Invoke();
				return new OkObjectResult(new ApiResponseT<T> { Code = 0, Message = string.Empty, ReturnValue = serviceDelegate.Invoke() });
			}
			catch (Exception exception)
			{
				return HandleException(exception);
			}
		}

		protected IActionResult HandleException(Exception exception)
		{
			switch (exception)
			{
				case BaseException baseException:
					//return new OkObjectResult(new ApiResponse(baseException.ErrorCode, baseException.Message));
					return StatusCode(
						(int)baseException.HttpCode,
						new ApiResponse(baseException.ErrorCode, baseException.Message));

				case AggregateException aggregateException:
					static string SelectorMessage(Exception r) => r is BaseException baseException ? baseException.Message : r.Message;

					return StatusCode(
						(int)HttpStatusCode.InternalServerError,
						new ApiResponseT<IEnumerable<string>>(
							Core.ErorrCodes.General.AggregateException,
							aggregateException.Message,
							aggregateException.Flatten().InnerExceptions.Select(SelectorMessage)));

				default:
					//Log.Error(exception, "Exception Occured!");
					//HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					return StatusCode(
						(int)HttpStatusCode.InternalServerError,
						new ApiResponse(
							Core.ErorrCodes.General.InternalError,
							exception?.Message ?? "Internal error"));
			}
		}

#pragma warning restore CA1031 // Do not catch general exception types
	}
}