using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;

namespace SeedApp.Common.Exceptions
{
	public class SeedAppApiExceptions : SeedAppExceptions
	{
		public SeedAppApiExceptions(string errorCode, string errorMessage, Exception innerException = null) : base(errorCode, errorMessage, innerException)
		{
			ApiErrorCodes code;
			ApiErrorCode = Enum.TryParse(errorCode, out code) ? code : ApiErrorCodes.Unknown;
		}

		public SeedAppApiExceptions(HttpStatusCode httpStatusCode, string errorCode, string errorMessage, Exception innerException = null) : base(errorCode, errorMessage, innerException)
		{
			HttpStatusCode = httpStatusCode;
			ApiErrorCodes code;
			ApiErrorCode = Enum.TryParse(errorCode, out code) ? code : ApiErrorCodes.Unknown;
		}

		public SeedAppApiExceptions(ApiErrorCodes apiErrorCode, string errorMessage, Exception innerException = null) : base(apiErrorCode.ToString(), errorMessage, innerException)
		{
			ApiErrorCode = apiErrorCode;
		}

		public SeedAppApiExceptions(ApiErrorCodes apiErrorCode, string errorCode, string errorMessage, Exception innerException = null) : base(errorCode, errorMessage, innerException)
		{
			ApiErrorCode = apiErrorCode;
		}

		public SeedAppApiExceptions(ApiErrorCodes apiErrorCode, HttpStatusCode httpStatusCode, string errorCode, string errorMessage, Exception innerException = null) : base(errorCode, errorMessage, innerException)
		{
			ApiErrorCode = apiErrorCode;
			HttpStatusCode = httpStatusCode;
		}

		public SeedAppApiExceptions(ApiErrorCodes apiErrorCode, HttpStatusCode httpStatusCode, string errorCode, string errorMessage, string responseContent, Exception innerException = null) : base(errorCode, errorMessage, innerException)
		{
			ApiErrorCode = apiErrorCode;
			HttpStatusCode = httpStatusCode;
			ResponseContent = responseContent;
		}

		public SeedAppApiExceptions(HttpStatusCode httpStatusCode, string errorCode, string errorMessage, string responseContent, Exception innerException = null) : base(errorCode, errorMessage, innerException)
		{
			ApiErrorCodes code;
			ApiErrorCode = Enum.TryParse(errorCode, out code) ? code : ApiErrorCodes.Unknown;
			HttpStatusCode = httpStatusCode;
			ResponseContent = responseContent;
		}

		public HttpStatusCode HttpStatusCode { get; set; }

		public ApiErrorCodes ApiErrorCode { get; set; }

		public string ResponseContent { get; set; }


		public static SeedAppApiExceptions ProcessApiException(Exception ex, int? id = null, string api = null, string verb = null, string url = null, object data = null)
		{
			WebException webEx = ex as WebException;

			bool isTimeout = ex is TaskCanceledException || (webEx != null && webEx.Status != WebExceptionStatus.UnknownError);
			bool isOnline = ((IConnectivityHelper)ContainerManager.Container.Resolve(typeof(IConnectivityHelper), typeof(IConnectivityHelper).GetType().ToString())).IsConnected;

			if (id != null)
				((ILogger)ContainerManager.Container.Resolve(typeof(ILogger),
															 typeof(ILogger).GetType().ToString()))
					.Error($"InvocationId<{id}> Request to {api} failed",
						   new { ex.Message, isOnline, isTimeout, verb, url, data },
						   new[] { LoggerConstants.ApiOperation });

			Dictionary<string, object> metadata = new Dictionary<string, object>
				{
					{"verb", verb},
					{"url", url},
					{"data", data},
					{"isOnline", isOnline},
					{"isTimedOut", isTimeout}
				};

			if (webEx != null)
				metadata.Add("WebExceptionStatus", webEx.Status.ToString());

			if (!isOnline)
				return new SeedAppApiExceptions(ApiErrorCodes.ConnectivityLost, ex.Message);

			if (isTimeout)
				return new SeedAppApiExceptions(ApiErrorCodes.ServerUnreachable, ex.Message);

			return new SeedAppApiExceptions(ApiErrorCodes.Unknown, ex.Message);
		}
	}
}
