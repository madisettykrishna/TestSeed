using System;
using System.Threading.Tasks;
using SeedApp.Common.Enums;

namespace SeedApp.Common.Interfaces
{
	public interface IConnectivityHelper
	{
		bool IsConnected { get; }

		NetworkStatus NetworkStatus { get; }

		Task InitiateCheckingAsync();

		void ContinueChecking();

		void PauseChecking();

		Task SetConnectionAsync();
	}
}
