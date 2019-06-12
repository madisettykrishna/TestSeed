using SeedApp.Common.Data;
using SeedApp.Common.Models;
using SQLite.Net;

namespace SeedApp.Data
{
    public class AppDatabase : Database, IAppDatabase
    {
        public AppDatabase(IMemberPlusDataConfig dataConfig)
        {
            Connection = new SQLiteConnectionWithLock(dataConfig.SqLitePlatform, new SQLiteConnectionString(dataConfig.AppDataDatabasePath, true));
            Connection.CreateTable<CurrentUser>();
            Connection.CreateTable<SyncMetadata>();
        }
    }
}