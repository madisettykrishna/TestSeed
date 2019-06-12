using SQLite.Net.Interop;

namespace SeedApp.Common.Data
{
    public interface IMemberPlusDataConfig
    {
        string LogsDatabasePath { get; }

        ISQLitePlatform SqLitePlatform { get; }

        string AppContactsDatabasePath { get; }

        string AppDataDatabasePath { get; }
    }
}