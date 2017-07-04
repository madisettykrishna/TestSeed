using System;
using SeedApp.Common.Interfaces;
using SQLite.Net;

namespace SeedApp.Common.DB
{
	public class SqLiteConnectionFactory : ISqLiteConnectionFactory
	{
		private readonly SQLiteConnectionWithLock _appDBConnectionWithLock;
		private readonly SQLiteConnectionWithLock _logsDBConnectionWithLock;

		public SqLiteConnectionFactory(IAppConfig appConfig)
		{			
			_appDBConnectionWithLock = new SQLiteConnectionWithLock(appConfig.SqLitePlatform, new SQLiteConnectionString(appConfig.AppDatabaseConnectionString, true));

			_logsDBConnectionWithLock = new SQLiteConnectionWithLock(appConfig.SqLitePlatform, new SQLiteConnectionString(appConfig.LogDatabaseConnectionString, true));
		}

		public SQLiteConnectionWithLock GetLogsConnection()
		{
			return _logsDBConnectionWithLock;
		}

		public SQLiteConnectionWithLock GetAppDBConnection()
		{
			return _appDBConnectionWithLock;
		}
	}
}
