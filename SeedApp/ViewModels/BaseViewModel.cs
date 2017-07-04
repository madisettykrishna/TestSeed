using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using XLabs.Forms.Mvvm;

namespace SeedApp.Forms.ViewModels
{
	public class BaseViewModel : ViewModel
	{
		protected readonly INavigationService NavigationService;
		protected readonly ILogger Logger;

		public BaseViewModel(INavigationService navigationService, ILogger logger)
		{
			NavigationService = navigationService;
			Logger = logger;
		}
	}
}
