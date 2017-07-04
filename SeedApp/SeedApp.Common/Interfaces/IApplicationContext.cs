using SeedApp.Common.Logging;

namespace SeedApp.Common.Interfaces
{
	public interface IApplicationContext
	{
		string CurrentLoggedInUserName { get; set; }

		LogLevel LogLevel { get; set; }
	}
}
