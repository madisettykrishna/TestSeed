using SeedApp.Common.Logging;
using SeedApp.Data.Interfaces;

namespace SeedApp.Data.Repositories
{
	public class RepositoryBase
	{
		public RepositoryBase(IServiceClientDatabase database, ILogger logger)
		{
			Database = database;
			Logger = logger;
		}

		protected IServiceClientDatabase Database { get; set; }

		protected ILogger Logger { get; set; }
	}
}
