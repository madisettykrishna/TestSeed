using System.Collections.Generic;
using SeedApp.Common.Models;

namespace SeedApp.Common.Interfaces
{
    public interface IAppAnalyticsProvider
    {
        void Initialize();

        void ReportException(System.Exception ex, string[] tags = null, Dictionary<string, object> data = null);

        void ReportApiTiming(string api, long timeInMilliSecs);
    }
}