using System.Threading.Tasks;
using Newtonsoft.Json;
using SeedApp.Common.Entities;
using SeedApp.Common.Interfaces;
using Xamarin.Forms;

namespace SeedApp.Common.Logging
{
	public class BaseLogginProvider
	{
		protected readonly IPlatformService _platformService;
		protected readonly IApplicationContext _applicationContext;

		public BaseLogginProvider(IApplicationContext applicationContext, IPlatformService platformServices)
		{
			_applicationContext = applicationContext;
			_platformService = platformServices;
		}

		protected LogEntry CreateLogEntry(string message,
			LogLevel level,
			object data,
			string[] tags,
			string callerFullTypeName,
			string callerMemberName,
			long? timeInMilliseconds = null)
		{
			var report = new LogEntry();

			if (tags != null)
			{
				report.Tags = string.Join(", ", tags);
			}

			report.TaskId = TaskScheduler.Current.Id;
			report.ThreadId = _platformService.CurrentThreadId;
			report.CallerMemberName = callerMemberName;
			report.CallerFullTypeName = callerFullTypeName;
			report.Data = data == null ? null : JsonConvert.SerializeObject(data);
			report.Level = level.ToString();
			report.Message = message;
			report.TimeInMilliseconds = timeInMilliseconds;
			report.UserName = _applicationContext.CurrentLoggedInUserName;
			report.Version = _platformService.AppVersion;
			report.Platform = Device.OS.ToString();

			var build = string.Empty;
#if ADHOC
            build = "adhoc";
#else
			build = "debug";
#endif

			report.App = _platformService.AppName.ToLower();

			report.SourceIp = _platformService.IpAddress;

			return report;
		}
	}
}
