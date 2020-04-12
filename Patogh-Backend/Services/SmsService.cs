namespace PatoghBackend.Services
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;

	using PatoghBackend.Contract;
	using PatoghBackend.Core;
	using PatoghBackend.Dto.User;
	using PatoghBackend.Exceptions.General;
	using PatoghBackend.Services.Common;

	public class SmsService : Service, ISmsService
	{
		public async Task<string> SendSms(PhoneNumberWrapper phoneNumber, string message)
		{
			if (string.IsNullOrEmpty(phoneNumber?.PhoneNumber))
			{
				throw new ArgumentNullException(nameof(phoneNumber));
			}

			if (string.IsNullOrEmpty(message))
			{
				throw new ArgumentNullException(nameof(message));
			}

			string fullResponse, responseCode, responseRef;
			StreamReader sr = null;
			try
			{
				WebRequest req = WebRequest.Create(Settings.Services.ParsGreenSms);
				Dictionary<string, string> dic = new Dictionary<string, string>
				{
					{ "signature", Settings.Services.ParsGreenSingnature },
					{ "to", phoneNumber.PhoneNumber },
					{ "text", message }
				};

				var prarameters = dic.Select(x => $"{x.Key}={x.Value}");
				string postData = string.Join("&", prarameters);
				byte[] send = Encoding.UTF8.GetBytes(postData);

				req.Method = "POST";
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = send.Length;

				Stream sout = req.GetRequestStream();
				sout.Write(send, 0, send.Length);
				sout.Flush();
				sout.Close();

				WebResponse res = await req.GetResponseAsync();
				sr = new StreamReader(res.GetResponseStream());

				fullResponse = sr.ReadToEnd();

				var arr = fullResponse.Split(';');
				if (arr.Length != 3 || arr[1] != "0")
				{
					throw new ExternalApiException(
						nameof(Settings.Services.ParsGreenSms),
						fullResponse);
				}

				responseCode = arr[1];
				responseRef = arr[2];

				return responseRef;
			}
			catch (IOException e)
			{
				throw;
			}
			finally
			{
				sr?.Close();
			}
		}

		private bool disposed = false; // To detect redundant calls

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
				}

				disposed = true;
				base.Dispose(disposing);
			}
		}
	}
}