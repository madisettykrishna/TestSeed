using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SeedApp.Common.Logging
{
	public interface ILogger
	{
		ILoggingProvider[] LoggingProviders { get; }

		void End(TimeSpan? elapsed = null, object data = null, string[] tags = null, [CallerMemberName] string callerName = null, string callerFullTypeName = null);

		void Error(
			string message,
			object data = null,
			string[] tags = null,
			[CallerMemberName] string callerName = null, string callerFullTypeName = null);

		void Exception(Exception exception, string[] tags = null, [CallerMemberName] string callerName = null, string callerFullTypeName = null, string message = null);

		void Info(string message, object data = null, string[] tags = null, [CallerMemberName] string callerName = null, string callerFullTypeName = null);

		void Start(object data = null, string[] tags = null, [CallerMemberName] string callerName = null, string callerFullTypeName = null);

		void Verbose(
			string message,
			object data = null,
			string[] tags = null,
			[CallerMemberName] string callerName = null, string callerFullTypeName = null);

		void Warning(
			string message,
			object data = null,
			string[] tags = null,
			[CallerMemberName] string callerName = null, string callerFullTypeName = null);

		Task ClearLogsAsync();
	}
}
