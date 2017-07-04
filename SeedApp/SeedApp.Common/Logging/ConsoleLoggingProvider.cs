using System.Diagnostics;
using System.Threading.Tasks;
using SeedApp.Common.Interfaces;

namespace SeedApp.Common.Logging
{
    public class ConsoleLoggingProvider : BaseLogginProvider, ILoggingProvider
	{
		public ConsoleLoggingProvider(IApplicationContext applicationContext, IPlatformService platformService) : base(applicationContext, platformService)
		{
		}

		public async Task ClearLogsAsync()
		{
		}

		public void Log(
			string message,
			LogLevel level,
			object data,
			string[] tags,
			string callerFullTypeName,
			string callerMemberName,
			long? timeInMilliseconds = null)
		{
			var report = CreateLogEntry(message, level, data, tags, callerFullTypeName, callerMemberName,
				timeInMilliseconds);

			Debug.WriteLine(report);
		}
	}
}
