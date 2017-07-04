using System;
using SeedApp.Common.Interfaces;
using SeedApp.Common.DB;

namespace SeedApp.Common.Logging
{
	public class LogsDatabase : AppDatabase, ILogsDatabase
	{
		public LogsDatabase(ILogsInternalDatabase database) : base(database)
		{
		}
	}
}
