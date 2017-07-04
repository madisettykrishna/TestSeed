using System;
namespace SeedApp.Common.Exceptions
{
	public class SeedAppExceptions : Exception
	{
		public SeedAppExceptions(string errorCode, string errorMessage, Exception innerException = null) : base($"ErrorCode={errorCode}, ErrorMessage={errorMessage}", innerException)
		{
			ErrorCodeAsString = errorCode ?? string.Empty;
            ErrorMessage = errorMessage;
		}

		public string ErrorCodeAsString { get; set; }

		public string ErrorMessage { get; set; }
	}
}
