namespace SeedApp.Common.Interfaces
{
    public interface IPeriodicSyncManager
    {
        void InitiateSyncing();

        void PauseSyncing();

        void ContinueSyncing();
    }
}
