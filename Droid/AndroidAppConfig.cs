using System;
using System.IO;
using SeedApp.Common;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;

namespace SeedApp.Forms.Droid
{
	public class AndroidAppConfig : AppConfig
	{
		private string _connectionString;
		private string _logsConnectionString;

		public override string ApplicationDatabaseFileName => "AppName.db";

		public override ISQLitePlatform SqLitePlatform => new SQLitePlatformAndroid();

		public override string StoreAppRaygunKey => "PlayStore App Raygun key goes here";

		public override string BetaAppRaygunKey => "Beta App Raygun key goes here";

		public override string TestAppRaygunKey => "Test App Raygun key goes here";

		public override string GoogleAnalyticsId => "Not required in Android project";

		public override string AppDatabaseConnectionString
		{
			get
			{
				if (!string.IsNullOrEmpty(_connectionString))
					return _connectionString;
				_connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
												 ApplicationDatabaseFileName);
				return _connectionString;
			}
		}

		public override string LogDatabaseConnectionString
		{
			get
			{
				if (!string.IsNullOrEmpty(_logsConnectionString))
					return _logsConnectionString;
				_logsConnectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
													LogsDatabaseFilename);
				return _logsConnectionString;
			}
		}

		public override string LogsDatabaseFilename
		{
			get
			{
				return base.LogsDatabaseFilename;
			}
		}
	}
}
