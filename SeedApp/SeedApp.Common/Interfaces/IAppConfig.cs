using System;
using SQLite.Net.Interop;

namespace SeedApp.Common.Interfaces
{
	public interface IAppConfig
	{
		string ApiBaseUrl { get; }

		string ApplicationDatabaseFileName { get; }

		string LogsDatabaseFilename { get; }

		string AppDatabaseConnectionString { get; }

		string LogDatabaseConnectionString { get; }

		string StoreAppRaygunKey { get; }

		string BetaAppRaygunKey { get; }

		string TestAppRaygunKey { get; }

		string GoogleAnalyticsId { get; }

		ISQLitePlatform SqLitePlatform { get; }
	}
}
