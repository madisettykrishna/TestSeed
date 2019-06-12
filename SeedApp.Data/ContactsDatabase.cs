using SeedApp.Common.Data;
using SeedApp.Common.Models;
using SQLite.Net;

namespace SeedApp.Data
{
    public class ContactsDatabase : Database, IContactsDatabase
    {
        public ContactsDatabase(IMemberPlusDataConfig dataConfig)
        {
            Connection = new SQLiteConnectionWithLock(dataConfig.SqLitePlatform, new SQLiteConnectionString(dataConfig.AppContactsDatabasePath, true));
            Connection.CreateTable<SyncMetadata>();
        }
    }
}