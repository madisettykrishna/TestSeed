using SeedApp.Common.DB;
using SeedApp.Common.Interfaces;
using SeedApp.Data.Entities;
using SeedApp.Data.Interfaces;

namespace SeedApp.Data.Database
{
    public class ServiceClientInternalDatabase : AppInternalDatabase, IServiceClientInternalDatabase
	{
		public ServiceClientInternalDatabase(ISqLiteConnectionFactory connectionFactory) : base(connectionFactory.GetAppDBConnection())
		{
			// Create tables here, using following Way,
			Connection.CreateTable<TestEntity>();
		}
	}
}
