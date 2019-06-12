using System;
using System.Threading.Tasks;
using SeedApp.Common.Data;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SeedApp.Data.Logging
{
    public class LocalDbLoggingProvider : ILoggingProvider
    {
        private readonly ILogDatabase _logDatabase;
        private readonly IPlatformService _platformService;

        public LocalDbLoggingProvider(ILogDatabase logDatabase, IPlatformService platformService)
        {
            _logDatabase = logDatabase;
            _platformService = platformService;
        }

        public void Log(string message, LogLevel level, object data, string[] tags, string callerFullTypeName,
            string callerMemberName)
        {
            LogEntry log = new LogEntry
            {
                Message = message,
                Level = level.ToString(),
                CallerMemberName = callerMemberName,
                CallerFullTypeName = callerFullTypeName,
                Data = data == null ? null : JsonConvert.SerializeObject(data),
                TaskId = TaskScheduler.Current.Id,
                CreatedOn = DateTime.UtcNow.ToString("dd-MMMM-yy, hh:mm:ss"),
                Version = _platformService.AppVersion
            };

            if (tags != null)
            {
                log.Tags = string.Join(", ", tags);
            }

            _logDatabase.Insert(log);
            Debug.WriteLine(log.ToString());
        }
    }
}