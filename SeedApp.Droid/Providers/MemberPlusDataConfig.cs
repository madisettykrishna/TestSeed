using System;
using System.IO;
using SeedApp.Common.Data;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;

namespace SeedApp.Droid
{
    public class MemberPlusDataConfig : IMemberPlusDataConfig
    {
        public string LogsDatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "logs.db3");

        public string AppContactsDatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "contactsData.db3");

        public string AppDataDatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "appData.db3");

        public ISQLitePlatform SqLitePlatform => new SQLitePlatformAndroidN();
    }
}
