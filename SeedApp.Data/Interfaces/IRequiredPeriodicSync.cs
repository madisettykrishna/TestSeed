using System.Threading.Tasks;

namespace SeedApp.Data.Interfaces
{
    public interface IRequirePeriodicSync
    {
        int IntervalInSecs { get; }

        string FriendlyNameForLogs { get; }

        Task SynchronizeAsync();
    }
}