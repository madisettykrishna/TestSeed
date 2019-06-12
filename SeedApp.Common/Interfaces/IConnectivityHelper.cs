using System.Threading.Tasks;

namespace SeedApp.Common.Interfaces
{
    public interface IConnectivityHelper
    {
        bool IsConnected { get; }

        Task InitiateCheckingAsync();

        void ContinueChecking();

        void PauseChecking();
    }
}