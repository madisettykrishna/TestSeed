using System;
using SeedApp.Common.Interfaces;
using SQLite.Net.Interop;

namespace SeedApp.Common
{
	public class AppConfig : IAppConfig
	{
		private readonly string _logsDatabaseFileName = "Logs.db";

		public virtual string LogsDatabaseFilename => _logsDatabaseFileName;

		public virtual string ApplicationDatabaseFileName
		{
			get;
		}

		public virtual string ClientId
		{
			get;
		}

		public string ApiBaseUrl => "Some Api url goes here";

		public virtual string AppDatabaseConnectionString
		{
			get;
		}

		public virtual string LogDatabaseConnectionString
		{
			get;
		}

		public virtual string GoogleAnalyticsId
		{
			get;
		}

		public virtual string StoreAppRaygunKey
		{
			get;
		}

		public virtual string BetaAppRaygunKey
		{
			get;
		}

		public virtual string TestAppRaygunKey
		{
			get;
		}

		public virtual ISQLitePlatform SqLitePlatform
		{
			get;
		}
	}
}
