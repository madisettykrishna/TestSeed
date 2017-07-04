using System;
namespace SeedApp.Common.Interfaces
{
	public interface IGoogleAnalyticsServices
	{
		void SendPage(string pageName);

		void SendException(Exception ex, string extraMessage = null);

		void SetAppVersion(string version);

		void SendTiming(string category, long milliseconds, string name = null, string label = null);

		void SendEvent(string category, string action, string label, long? value);
	}
}
