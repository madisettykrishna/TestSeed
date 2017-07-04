using System;
using SeedApp.Common.DB;
using SeedApp.Data.Interfaces;

namespace SeedApp.Data.Database
{
	public class ServiceClientDatabase : AppDatabase, IServiceClientDatabase
	{
		public ServiceClientDatabase(IServiceClientInternalDatabase database) : base(database)
		{
		}
	}
}
