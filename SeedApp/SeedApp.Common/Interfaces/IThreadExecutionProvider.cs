using System;
using System.Threading.Tasks;

namespace SeedApp.Common.Interfaces
{
	public interface IThreadExecutionProvider
	{
		void RunOnMainThread(Action action);

		Task RunOnThreadPoolThreadAsync(Action action);

		/// <summary>
		/// The method allows to run a task on background even after app is closed on iOS. This method is not implemented for other platforms
		/// </summary>
		/// <param name="action">The action to be executed on as background task</param>
		/// <param name="canShowNetworkIndicator">Indicates whether network indicator has to be shown on the status bar or not</param>
		/// <param name="finishWithDelay">Time in seconds to wait before cancelling the background task</param>
		void RunOnBackgroundThread(Action action, bool canShowNetworkIndicator, double finishWithDelay = 0);
	}
}
