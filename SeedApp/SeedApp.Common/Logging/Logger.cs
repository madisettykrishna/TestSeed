using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SeedApp.Common.Interfaces;

namespace SeedApp.Common.Logging
{
    public class Logger : ILogger
	{
		private readonly IStackFrameHelper _stackFrameHelper;

		public Logger(ILoggingProvider[] providers,
		              IStackFrameHelper stackFrameHelper)
		{
			_stackFrameHelper = stackFrameHelper;
			LoggingProviders = providers;
		}

		public ILoggingProvider[] LoggingProviders { get; }

		public void Start(object data = null, string[] tags = null, [CallerMemberName] string callerName = null,
			string callerFullTypeName = null)
		{
			int classNameStartIndex = _stackFrameHelper.GetCallerFullTypeName().LastIndexOf('.') + 1;
			int classNameLength = _stackFrameHelper.GetCallerFullTypeName().LastIndexOf('+') - classNameStartIndex;
			string callingClassName = classNameLength > 0 ? _stackFrameHelper.GetCallerFullTypeName().Substring(classNameStartIndex, classNameLength) : string.Empty;
			foreach (var eachProvider in LoggingProviders)
			{
				var msg = string.IsNullOrEmpty(callingClassName) ? $"Starting method {callerName}" : $"Starting method {callingClassName}.{callerName}";
				eachProvider.Log(msg,
					LogLevel.Verbose,
					PrepareData(data),
					tags,
					callerFullTypeName ?? _stackFrameHelper.GetCallerFullTypeName(),
					callerName);
			}
		}

		public void End(TimeSpan? elapsed = null, object data = null, string[] tags = null,
			[CallerMemberName] string callerName = null, string callerFullTypeName = null)
		{
			int classNameStartIndex = _stackFrameHelper.GetCallerFullTypeName().LastIndexOf('.') + 1;
			int classNameLength = _stackFrameHelper.GetCallerFullTypeName().LastIndexOf('+') - classNameStartIndex;
			string callingClassName = classNameLength > 0 ? _stackFrameHelper.GetCallerFullTypeName().Substring(classNameStartIndex, classNameLength) : string.Empty;
			foreach (var eachProvider in LoggingProviders)
			{
				var msg = string.IsNullOrEmpty(callingClassName) ? $"Ending method {callerName}" : $"Ending method {callingClassName}.{callerName}";
				eachProvider.Log(msg,
					LogLevel.Verbose,
					PrepareData(data),
					tags,
					callerFullTypeName ?? _stackFrameHelper.GetCallerFullTypeName(),
					callerName,
					elapsed != null ? (long)elapsed.Value.TotalMilliseconds : (long?)null);
			}
		}

		public void Verbose(
			string message,
			object data = null,
			string[] tags = null,
			[CallerMemberName] string callerName = null,
			string callerFullTypeName = null)
		{
			foreach (var provider in LoggingProviders)
			{
				provider.Log(message,
					LogLevel.Verbose,
					PrepareData(data),
					tags,
					callerFullTypeName ?? _stackFrameHelper.GetCallerFullTypeName(),
					callerName);
			}
		}

		public void Info(
			string message,
			object data = null,
			string[] tags = null,
			[CallerMemberName] string callerName = null, string callerFullTypeName = null)
		{
			foreach (var provider in LoggingProviders)
			{
				provider.Log(message,
					LogLevel.Info,
					PrepareData(data),
					tags,
					callerFullTypeName ?? _stackFrameHelper.GetCallerFullTypeName(),
					callerName);
			}
		}

		public void Warning(
			string message,
			object data = null,
			string[] tags = null,
			[CallerMemberName] string callerName = null, string callerFullTypeName = null)
		{
			foreach (var provider in LoggingProviders)
			{
				provider.Log(message,
					LogLevel.Warning,
					PrepareData(data),
					tags,
					callerFullTypeName ?? _stackFrameHelper.GetCallerFullTypeName(),
					callerName);
			}
		}

		public void Error(
			string message,
			object data = null,
			string[] tags = null,
			[CallerMemberName] string callerName = null, string callerFullTypeName = null)
		{
			foreach (var provider in LoggingProviders)
			{
				provider.Log(message,
					LogLevel.Error,
					PrepareData(data),
					tags,
					callerFullTypeName ?? _stackFrameHelper.GetCallerFullTypeName(),
					callerName);
			}
		}

		public void Exception(Exception exception, string[] tags = null, [CallerMemberName] string callerName = null,
			string callerFullTypeName = null, string message = null)
		{
			foreach (var provider in LoggingProviders)
			{
				var exceptionData = string.Empty;

				foreach (var key in exception.Data.Keys)
				{
					exceptionData += $"{key}={exception.Data[key]};";
				}

				var data = new
				{
					ExceptionMessage = string.IsNullOrEmpty(message) ? exception.Message : message,
					ExceptionSource = exception.Source,
					ExceptionStackTrace = exception.StackTrace,
					ExceptionInnerException = exception.InnerException != null ? exception.InnerException.Message : string.Empty,
					ExceptionData = exceptionData,
					ExceptionAdditionalData = exception.ToString()
				};

				provider.Log(exception.Message,
					LogLevel.Error,
					PrepareData(data),
					tags,
					callerFullTypeName ?? _stackFrameHelper.GetCallerFullTypeName(),
					callerName);
			}
		}

		public async Task ClearLogsAsync()
		{
			foreach (var provider in LoggingProviders)
			{
				await provider.ClearLogsAsync();
			}
		}

		private JObject PrepareData(object data)
		{
			if (data == null)
			{
				return new JObject();
			}

			JObject o;
			if (data is string)
			{
				o = new JObject { { "message", data.ToString() } };
			}
			else
			{
				o = JObject.FromObject(data);
			}

			return o;
		}
	}
}
