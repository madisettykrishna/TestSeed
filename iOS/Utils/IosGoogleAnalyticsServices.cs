using System;
using Foundation;
using Google.Analytics;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;

namespace SeedApp.Forms.iOS.Utils
{
	public class IosGoogleAnalyticsServices : IGoogleAnalyticsServices
	{
		private readonly Common.Logging.ILogger _logger;

		public IosGoogleAnalyticsServices(Common.Logging.ILogger logger)
		{
			_logger = logger;
		}

		public void SendPage(string pageName)
		{
			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, pageName);
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
		}

		public void SendException(Exception ex, string extraMessage = null)
		{
			Gai.SharedInstance.DefaultTracker.Send(
				DictionaryBuilder.CreateException(extraMessage ?? string.Empty + ex.Message, NSNumber.FromBoolean(false))
					.Build());
		}

		public void SetAppVersion(string version)
		{
			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.AppVersion, version);
		}

		public void SendTiming(string category, long milliseconds, string name = null, string label = null)
		{
			Gai.SharedInstance.DefaultTracker.Send(
				DictionaryBuilder.CreateTiming(category, NSNumber.FromInt64(milliseconds), name, label).Build());
		}

		public void SendEvent(string category, string action, string label, long? value)
		{
			Gai.SharedInstance.DefaultTracker.Send(
				DictionaryBuilder.CreateEvent(category,
					action,
					label,
					value.HasValue ? NSNumber.FromLong((nint)value) : null).Build());
		}
	}
}
