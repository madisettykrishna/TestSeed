using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace SeedApp.Common.Logging
{
    public class Logger : ILogger
    {
        private readonly IEnumerable<ILoggingProvider> _providers;
        private readonly IStackFrameProvider _stackFrameProvider;

        public Logger(IEnumerable<ILoggingProvider> providers, IStackFrameProvider stackFrameProvider)
        {
            _providers = providers;
            _stackFrameProvider = stackFrameProvider;
        }

        public void Verbose(
            string message,
            object data = null,
            string[] tags = null,
            [CallerMemberName] string callerName = null,
            string callerFullTypeName = null)
        {
            foreach (var provider in _providers)
            {
                provider.Log(message,
                    LogLevel.Verbose,
                    PrepareData(data),
                    tags,
                    callerFullTypeName ?? _stackFrameProvider.GetCallerFullTypeName(),
                    callerName);
            }
        }

        public void Info(
            string message,
            object data = null,
            string[] tags = null,
            [CallerMemberName] string callerName = null, string callerFullTypeName = null)
        {
            foreach (var provider in _providers)
            {
                provider.Log(message,
                    LogLevel.Info,
                    PrepareData(data),
                    tags,
                    callerFullTypeName ?? _stackFrameProvider.GetCallerFullTypeName(),
                    callerName);
            }
        }

        public void Warning(
            string message,
            object data = null,
            string[] tags = null,
            [CallerMemberName] string callerName = null, string callerFullTypeName = null)
        {
            foreach (var provider in _providers)
            {
                provider.Log(message,
                    LogLevel.Warning,
                    PrepareData(data),
                    tags,
                    callerFullTypeName ?? _stackFrameProvider.GetCallerFullTypeName(),
                    callerName);
            }
        }

        public void Error(
            string message,
            object data = null,
            string[] tags = null,
            [CallerMemberName] string callerName = null, string callerFullTypeName = null)
        {
            foreach (var provider in _providers)
            {
                provider.Log(message,
                    LogLevel.Error,
                    PrepareData(data),
                    tags,
                    callerFullTypeName ?? _stackFrameProvider.GetCallerFullTypeName(),
                    callerName);
            }
        }

        public void Exception(System.Exception exception, string[] tags = null, [CallerMemberName] string callerName = null,
            string callerFullTypeName = null)
        {
            foreach (var provider in _providers)
            {
                var exceptionData = string.Empty;

                foreach (var key in exception.Data.Keys)
                {
                    exceptionData += $"{key}={exception.Data[key]};";
                }

                var data = new
                {
                    ExceptionMessage = exception.Message,
                    ExceptionSource = exception.Source,
                    ExceptionStackTrace = exception.StackTrace,
                    ExceptionInnerException = exception.InnerException != null ? exception.InnerException.Message : string.Empty,
                    ExceptionData = exceptionData
                };

                provider.Log(exception.Message,
                    LogLevel.Error,
                    PrepareData(data),
                    tags,
                    callerFullTypeName ?? _stackFrameProvider.GetCallerFullTypeName(),
                    callerName);
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