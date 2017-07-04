using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;

namespace SeedApp.Forms.Services
{
	public class ApplicationContext : IApplicationContext
	{		
		public string CurrentLoggedInUserName { get; set; }

		public LogLevel LogLevel { get; set; }
	}
}
