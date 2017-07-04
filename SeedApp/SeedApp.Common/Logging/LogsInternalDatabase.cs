using System;
using SeedApp.Common.DB;
using SeedApp.Common.Entities;
using SeedApp.Common.Interfaces;

namespace SeedApp.Common.Logging
{
	public class LogsInternalDatabase : AppInternalDatabase, ILogsInternalDatabase
	{
		public LogsInternalDatabase(ISqLiteConnectionFactory connectionFactory) : base(connectionFactory.GetLogsConnection())
		{
			// Create tables if they don't exist.
			Connection.CreateTable<LogEntry>();

			Connection.CreateIndex<LogEntry>(x => x.SentToServerAtUtc);
		}
	}
}
