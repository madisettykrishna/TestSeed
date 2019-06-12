using SeedApp.Common.Data;
using SeedApp.Common.Models;
using SQLite.Net;

namespace SeedApp.Data
{
    public class LogDatabase : Database, ILogDatabase
    {
        public LogDatabase(IMemberPlusDataConfig dataConfig)
        {
            Connection = new SQLiteConnectionWithLock(dataConfig.SqLitePlatform, new SQLiteConnectionString(dataConfig.LogsDatabasePath, true));
            Connection.CreateTable<LogEntry>();
        }
    }
}